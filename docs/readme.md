# Introduction

# Steps

## Create GLU Tables
```bash
https://us-west-2.console.aws.amazon.com/cloudformation/home?region=us-west-2#/stacks/quickcreate?templateURL=https://covid19-lake.s3.us-east-2.amazonaws.com/cfn/CovidLakeStack.template.json&stackName=CovidLakeStack

aws cloudformation create-stack --stack-name covid-lake-stack --template-url https://covid19-lake.s3.us-east-2.amazonaws.com/cfn/CovidLakeStack.template.json
```

## Create bucket
```bash
aws cloudformation create-stack --stack-name athena-results-netcore --template-body file://s3-athena-result.template.yaml
```

## COVID-19 Analisys 

```sql
SELECT 
  cases.fips, 
  admin2 as county, 
  province_state, 
  confirmed,
  growth_count, 
  sum(num_licensed_beds) as num_licensed_beds, 
  sum(num_staffed_beds) as num_staffed_beds, 
  sum(num_icu_beds) as num_icu_beds
FROM 
  "covid-19"."hospital_beds" beds, 
  ( SELECT 
      fips, 
      admin2, 
      province_state, 
      confirmed, 
      last_value(confirmed) over (partition by fips order by last_update) - first_value(confirmed) over (partition by fips order by last_update) as growth_count,
      first_value(last_update) over (partition by fips order by last_update desc) as most_recent,
      last_update
    FROM  
      'covid-19'.'enigma_jhu' 
    WHERE 
      from_iso8601_timestamp(last_update) > now() - interval '7' day AND country_region = 'US') cases
WHERE 
  beds.fips = cases.fips AND last_update = most_recent
GROUP BY cases.fips, confirmed, growth_count, admin2, province_state
ORDER BY growth_count desc

--Testing and deaths
SELECT * FROM "covid-19"."world_cases_deaths_testing" order by "date" desc limit 10;

SELECT * FROM "covid-19"."nytimes_counties" order by "date" desc limit 10;

-- Testing
SELECT 
   date,
   positive,
   negative,
   pending,
   hospitalized,
   death,
   total,
   deathincrease,
   hospitalizedincrease,
   negativeincrease,
   positiveincrease,
   sta.state AS state_abbreviation,
   abb.state 

FROM "covid-19"."covid_testing_states_daily" sta
JOIN "covid-19"."us_state_abbreviations" abb ON sta.state = abb.abbreviation
limit 500;

SELECT * FROM "covid-19"."covid_testing_us_daily"  order by "date" desc limit 10;

```

# References
https://aws.amazon.com/blogs/big-data/a-public-data-lake-for-analysis-of-covid-19-data/
https://docs.aws.amazon.com/athena/latest/ug/code-samples.html
https://aws.amazon.com/blogs/apn/using-athena-express-to-simplify-sql-queries-on-amazon-athena/
https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html
https://docs.aws.amazon.com/sdk-for-java/v1/developer-guide/credentials.html

aws athena start-query-execution --query-string "SELECT * FROM `covid-19`.`covid_testing_us_daily`  order by `date` desc limit 10" --result-configuration OutputLocation=s3://athena-results-netcore-s3bucket-fk6joy3h5yof/athena/results/