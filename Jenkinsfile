pipeline {
    agent any

    environment {
        AWS_REGION = 'us-east-1' // AWS Region
        AWS_ACCOUNT_ID = '149536454064' // AWS Account ID
        ECR_REPO_NAME = 'bright_aid_api' // ECR repository name
        CLUSTER_NAME = 'bright-aid' // ECS Cluster Name
        SERVICE_NAME = 'bright-aid-service' // ECS Service Name
        TASK_DEFINITION_REVISION = 'bright-aid:4' // Task Definition Revision
        DESIRED_COUNT = 2 // Desired Count of ECS Tasks
        DOCKER_CREDENTIALS_ID = 'my-docker-hub-credentials'
        DOCKER_REPO_NAME = 'kdalling/bright_aid_api'
    }
    stages {

        stage('Checkout') {
            steps {
                echo "Checking out branch"
                checkout scm
            }
        }
        
        stage('Static Code Analysis with SonarQube') {
            steps {
                // Run SonarQube analysis
                withSonarQubeEnv('SonarQube') {
                    sh 'dotnet sonarscanner begin -k:"BrightAidAPI-Sonar" -d:sonar.host.url="http://localhost:9000" -d:sonar.login="sqa_dc78ac5cbec78abebcc957d6464ce96e1815aec2"'
                    sh 'dotnet build --configuration Release'
                    sh 'dotnet test --no-build'
                    sh 'dotnet sonarscanner end -d:sonar.login="sqa_dc78ac5cbec78abebcc957d6464ce96e1815aec2"'
                }
            }
        }
        
        stage('Build .NET Core Project') {
            steps {
                // Restores the NuGet packages for the .NET Core project
                echo 'Restoring dependencies...'
                sh 'dotnet restore'
                
                // Builds the project in Release configuration
                echo 'Building the project...'
                sh 'dotnet build --configuration Release'
                
                // Publishes the application to a specified output directory
                echo 'Publishing the application...'
                sh 'dotnet publish --configuration Release --output ./publish'
            }
        }

        stage('Run Tests') {
            steps {
                // Run unit tests with proper formatting and collect code coverage
                echo 'Running unit tests with code coverage...'
                sh 'dotnet test ./Tests/Tests.csproj --collect:"XPlat Code Coverage" --configuration Release --results-directory Tests/TestResults'

                // Publish the coverage report in Jenkins
                echo 'Publishing code coverage reports...'
                cobertura coberturaReportFile: '**/TestResults/**/coverage.cobertura.xml'
            }
        }


        stage('Deliver to Dockerhub') {
            steps {
                // Login to dockerhub using credentials stored in Jenkins
                withCredentials([usernamePassword(credentialsId: "${DOCKER_CREDENTIALS_ID}", usernameVariable: 'DOCKER_USERNAME', passwordVariable: 'DOCKER_PASSWORD')]) {
                    sh "echo ${DOCKER_PASSWORD} | docker login -u ${DOCKER_USERNAME} --password-stdin"
                }
                // build docker image
                sh 'docker build -f ./301247589_301276375_bright_aid_API/Dockerfile -t bright_aid_api .'
                // Tag the Docker image
                sh 'docker tag bright_aid_api:latest $DOCKER_REPO_NAME:dev'
                // push the docker image to dockerhub
                sh 'docker push $DOCKER_REPO_NAME:dev'
            }
        }

        stage('Deploy to DEV Env') {
            steps {
                // Pull the image from Docker Hub to the local machine
                sh 'docker pull $DOCKER_REPO_NAME:dev'
                // Stop and remove existing container if it exists
                sh 'docker ps -q -f name=bright_aid_api_container | grep -q . && docker stop bright_aid_api_container && docker rm bright_aid_api_container || echo "No existing container to stop and remove"'
                // Run the Docker container on the local machine.
                sh 'docker run -itd --name bright_aid_api_container -p 3002:8080 $DOCKER_REPO_NAME:dev'
            }
        }

        stage('Deploy to QAT Env') {
            steps {
                echo 'Mocked up QAT Env running successfully'
            }
        }
        
        stage('Deploy to Staging Env') {
            steps {
                echo 'Mocked up Staging Env running successfully'
            }
        }

        stage('Deploy to Production Env') {
            steps {
                // Authenticate the Docker client to AWS ECR
                withCredentials([usernamePassword(credentialsId: 'aws-credentials', usernameVariable: 'AWS_ACCESS_KEY_ID', passwordVariable: 'AWS_SECRET_ACCESS_KEY')]) {
                    sh '''
                     aws configure set aws_access_key_id $AWS_ACCESS_KEY_ID
                     aws configure set aws_secret_access_key $AWS_SECRET_ACCESS_KEY
                     aws configure set region $AWS_REGION
                     aws ecr get-login-password --region $AWS_REGION | docker login --username AWS --password-stdin $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com
                     '''
                }
                // Build the Docker image using the specified Dockerfile
                sh 'docker build -f ./301247589_301276375_bright_aid_API/Dockerfile -t bright_aid_api .'
                // Tag the Docker image
                sh 'docker tag bright_aid_api:latest $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/$ECR_REPO_NAME:latest'
                // Push the Docker image to AWS ECR
                sh 'docker push $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/$ECR_REPO_NAME:latest'
                // Update the ECS service with the new task definition
                sh '''
				 aws ecs update-service \
					 --cluster $CLUSTER_NAME \
					 --service $SERVICE_NAME \
					 --task-definition $TASK_DEFINITION_REVISION \
					 --desired-count $DESIRED_COUNT \
					 --force-new-deployment \
					 --region $AWS_REGION
				 '''
            }
        }
    }
}
