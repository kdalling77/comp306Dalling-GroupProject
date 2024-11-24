pipeline {
    agent any

    environment {
        
        AWS_REGION = 'us-east-1' // AWS Region
        AWS_ACCOUNT_ID = '149536454064' // AWS Account ID
        ECR_REPO_NAME = 'bright_aid_api' // ECR repository name
        CLUSTER_NAME = 'bright-aid' // ECS Cluster Name
        SERVICE_NAME = 'bright-aid-service' // ECS Service Name
        TASK_DEFINITION_REVISION = 'bright-aid:3' // Task Definition Revision
        DESIRED_COUNT = 2 // Desired Count of ECS Tasks
    }
    
    stages {
        
        stage('Checkout') {
            steps {
                // Checkout the code from the specified Git repository and branch
                checkout scmGit(branches: [[name: '*/master']], extensions: [], userRemoteConfigs: [[url: 'https://github.com/kdalling77/comp306Dalling-GroupProject.git']])
            }
        }

        stage('Build .NET Core Project') {
            steps {
                // Restores the NuGet packages for the .NET Core project
                sh 'dotnet restore'
                
                // Builds the project in Release configuration
                sh 'dotnet build --configuration Release'

                 // Publishes the application to a specified output directory
                sh 'dotnet publish --configuration Release --output ./publish'
            }
        }

       stage('Run Tests') {
            
           steps {
                // Create tests here
                 echo 'Mock up Tests here'
            }
       }
        
       stage('Build & Push image to AWS ECR') {

             steps{
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
             }
         }

         stage('Deploy to AWS ECS Service') {
            steps {
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
