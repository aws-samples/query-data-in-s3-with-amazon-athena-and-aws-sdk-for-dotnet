# How to use SQL to query data in S3 Bucket with Amazon Athena and AWS SDK for .NET

This Project provides a sample implementation that will show how to leverage [Amazon Athena](https://aws.amazon.com/athena/) from .NET Core Application using [AWS SDK for .NET](https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/welcome.html) to run standard SQL to analyze a large amount of data in [Amazon S3](https://aws.amazon.com/s3/).
To showcase a more realistic use-case, it includes a WebApp UI developed using [ReactJs](https://reactjs.org/). this WebApp contains components to demonstrate fetching COVID-19 data from API Server that uses AWS SDK for .NET to connect to Amazon Athena and run SQL Standard query from datasets on Amazon S3 files from a Data Lake account. This Data Lake account is an open data available on [Registry of Open Data on AWS](https://registry.opendata.aws/); here's the link to the Data Lake <https://registry.opendata.aws/aws-covid19-lake/>.

Those ReatJs Components call .NET Core API that runs Amazon Athena Query, get QueryExecutionId, check the execution status, and list results. Each menu presents different views.

**Menu option _Testing By Date_**: Shows a filter by Date that present a table with the following data: Date, State, Positive, Negative, Pending, Hospitalized, Death, Positive Increase

**Menu option _Testing By State_**: Shows a filter by State that present a table with the following data: Date, State, Positive, Negative, Pending, Hospitalized, Death Positive Increase

**Menu option _Hospitals (Run&Go)_**: Run a request to the API server, get 200 with the Query ID, check the status of the execution; when the execution it's completed, it presents a table with the following data: Name, State, Type, ZipCode, Licenced Beds, Staffed Beds, Potential Increase in Beds

**Menu option _Hospitals (Run&Go)_**: Run request to the API server, wait for the result and present a table with the following data: Name, State, Type, Zip Code, Licenced Beds, Staffed Beds, Potential Increase in Beds

# Steps

To run this project follow the instructions bellow:

## 1) Deploy Glue Catalog & Athena Database/Tables

```bash
#1) Deploy
aws cloudformation create-stack --stack-name covid-lake-stack --template-url https://covid19-lake.s3.us-east-2.amazonaws.com/cfn/CovidLakeStack.template.json --region us-west-2

#2) Check deployment Status
aws cloudformation  describe-stacks --stack-name covid-lake-stack --region us-west-2
```

Below the result of status check, wait for **"StackStatus": "CREATE_COMPLETE"**

```json
{
    "Stacks": [
        {
            "StackId": "arn:aws:cloudformation:us-west-2:XXXXXXXX9152:stack/covid-lake-stack/xxxxxxxx-100d-11eb-87ef-xxxxxxxxxxx",
            "StackName": "covid-lake-stack",
            "CreationTime": "2020-10-17T00:12:09.151Z",
            "RollbackConfiguration": {},
            "StackStatus": "CREATE_COMPLETE",
            "DisableRollback": false,
            "NotificationARNs": [],
            "Tags": [],
            "EnableTerminationProtection": false,
            "DriftInformation": {
                "StackDriftStatus": "NOT_CHECKED"
            }
        }
    ]
}
```

## 2) Create S3 bucket for Athena Result

```bash
#1) Deploy S3 Bucket
aws cloudformation create-stack --stack-name athena-results-netcore --template-body file://s3-athena-result.template.yaml --region us-west-2

#2) Check deployment Status
aws cloudformation  describe-stacks --stack-name athena-results-netcore --region us-west-2
```

Below the result of status check, wait for **"StackStatus": "CREATE_COMPLETE"** and copy output Bucket Name **"OutputValue": "s3://athena-results-netcore-s3bucket-xxxxxxxxxxxx/athena/results/",** you will need this to run your code

```json
{
    "Stacks": [
        {
            "StackId": "arn:aws:cloudformation:us-west-2:XXXXXXXX9152:stack/athena-results-netcore/xxxxxxxx-100c-11eb-889f-xxxxxxxxxxx",
            "StackName": "athena-results-netcore",
            "Description": "Amazon S3 bucket to store Athena query results",
            "CreationTime": "2020-10-17T00:02:44.968Z",
            "LastUpdatedTime": "2020-10-17T00:21:13.692Z",
            "RollbackConfiguration": {
                "RollbackTriggers": []
            },
            "StackStatus": "CREATE_COMPLETE",
            "DisableRollback": false,
            "NotificationARNs": [],
            "Outputs": [
                {
                    "OutputKey": "BucketName",
                    "OutputValue": "s3://athena-results-netcore-s3bucket-xxxxxxxxxxxx/athena/results/",
                    "Description": "Name of the Amazon S3 bucket to store Athena query results"
                }
            ],
            "Tags": [],
            "EnableTerminationProtection": false,
            "DriftInformation": {
                "StackDriftStatus": "NOT_CHECKED"
            }
        }
    ]
}
```

## 3) COVID-19 Analisys (optional)

Some SQL Query you can try by your own using Amazon Athena UI

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
      from_iso8601_timestamp(last_update) > now() - interval '200' day AND country_region = 'US') cases
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

## 4) Build & Run .NET Web Application

1) Go to the app root dir

```bash
cd ./src/app/AthenaNetCore/
```

2) Create AWS Credential file, **_for security precaution the file extension *.env is added to .gitignore to avoid accidental commit_**

```bash
vi aws-credentials-do-not-commit.env #You can use any text editor eg: vscode -> code aws-credentials-do-not-commit.env
```

Below example of env file content, replace the XXXX... with your real AWS Credential, and add to S3_RESULT the output result you got from steep 2)

```ini
AWS_DEFAULT_REGION=us-west-2
AWS_ACCESS_KEY_ID=XXXXXXXXXXXXXXXXXXXX
AWS_SECRET_ACCESS_KEY=XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
AWS_SESSION_TOKEN=XXXXX #(Optional, used only in case of temporary token, you'll need to remove this comment on the .env file)
S3_RESULT_BUCKET_NAME=s3://athena-results-netcore-s3bucket-xxxxxxxxxxxx/athena/results/

```

3) Build .NET APP using docker-compose

```bash
docker-compose -f ./docker-compose.yml build
```

4) Run .NET APP docker-compose 

```bash
docker-compose -f ./docker-compose.yml up
```

5) Test .NET APP via URL <http://localhost:8089/>

# References

<https://aws.amazon.com/blogs/big-data/a-public-data-lake-for-analysis-of-covid-19-data/> 

<https://docs.aws.amazon.com/athena/latest/ug/code-samples.html>

<https://aws.amazon.com/blogs/apn/using-athena-express-to-simplify-sql-queries-on-amazon-athena/>

<https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html>

<https://docs.aws.amazon.com/sdk-for-java/v1/developer-guide/credentials.html>

<https://docs.aws.amazon.com/sdk-for-net/latest/developer-guide/creds-assign.html>

<https://github.com/awsdocs/aws-cloud9-user-guide/blob/master/LICENSE-SAMPLECODE>