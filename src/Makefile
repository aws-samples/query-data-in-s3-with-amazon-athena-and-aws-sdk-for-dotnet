# Input parameters
AWS_REGION = us-west-2
VPC_ID     = vpc-xxxxxxxxxxxxxxxxx
SUBNET_ID  = subnet-xxxxxxxxxxxxxxxxx

# Global variables
ACCOUNT_ID         = $(shell aws sts get-caller-identity --output text --query 'Account')
STACK_NAME         = athena-netcore-app
SOURCE_CODE_BUCKET = athena-examples-dotnetcore-$(ACCOUNT_ID)-$(AWS_REGION)

TIMESTAMP := $(shell date +%s)

clean:
	@echo "\n*** Cleaning ***"
	rm -f app-AthenaNetCore.zip
	
package: clean
	@echo "\n*** Packaging ***"
	zip -r app-AthenaNetCore.zip app

upload-s3: package
	@echo "\n*** Uploading to S3 ***"
	aws s3api head-bucket --bucket $(SOURCE_CODE_BUCKET) || aws s3 mb s3://$(SOURCE_CODE_BUCKET) --region $(AWS_REGION)
	aws s3 cp app-AthenaNetCore.zip s3://$(SOURCE_CODE_BUCKET)/

deploy: package upload-s3
	@echo "\n*** Deploying ***"
	sam deploy \
	  --region $(AWS_REGION) \
	  --stack-name $(STACK_NAME)-$(TIMESTAMP) \
	  --template-file cloud-formation-templates/docker-on-ec2.template.yaml \
	  --capabilities CAPABILITY_IAM \
	  --parameter-overrides \
	      VpcId=$(VPC_ID) \
	      SubnetId=$(SUBNET_ID) \
	      SourceCodeBucket=$(SOURCE_CODE_BUCKET)

.PHONY: clean package upload-s3 deploy
