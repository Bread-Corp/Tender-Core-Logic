# ⚙️ TenderTool Core Logic API — Central Business Hub

[![AWS Lambda](https://img.shields.io/badge/AWS-Lambda-orange.svg)](https://aws.amazon.com/lambda/)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![API Gateway](https://img.shields.io/badge/AWS-API%20Gateway-purple.svg)](https://aws.amazon.com/api-gateway/)
[![Amazon RDS](https://img.shields.io/badge/AWS-RDS-9d68c4.svg)](https://aws.amazon.com/rds/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2727.svg)](https://www.microsoft.com/sql-server/)
[![Entity Framework](https://img.shields.io/badge/EF%20Core-8.0-green.svg)](https://docs.microsoft.com/en-us/ef/)

**The intelligent command center of the TenderTool ecosystem!** 🧠 This .NET 8 Web API serves as the central orchestration hub, seamlessly connecting users to South Africa's most comprehensive tender intelligence platform. Deployed as a serverless AWS Lambda function, it's the secure, scalable brain that powers every user interaction and business decision.

## 📚 Table of Contents

- [✨ Key Features](#-key-features)
- [🧭 Architecture: The "Central Hub"](#-architecture-the-central-hub)
- [🧠 Core Responsibilities](#-core-responsibilities)
- [🧩 Project Structure](#-project-structure)
- [📦 Tech Stack](#-tech-stack)
- [⚙️ Configuration (Critical)](#️-configuration-critical)
- [🔒 Security: API Gateway & IAM](#-security-api-gateway--iam)
- [🗄️ Database: EF Core & Migrations](#️-database-ef-core--migrations)
- [🚀 Getting Started (Local Development)](#-getting-started-local-development)
- [📦 Deployment (CI/CD)](#-deployment-cicd)
- [🧰 Troubleshooting & Team Gotchas](#-troubleshooting--team-gotchas)

## ✨ Key Features

- **🎯 Centralized Business Logic**: The single source of truth for all user operations, watchlist management, and tender intelligence delivery
- **🎨 Service Orchestration Maestro**: Conducts a symphony of microservices (Mailer, Analytics, Logging) with precision and security
- **🔒 Fort Knox Security**: VPC-native deployment with enterprise-grade security through API Gateway and IAM authorization
- **⚡ Lightning-Fast Serverless**: .NET 8 Lambda function that scales instantly from zero to thousands of users
- **🛡️ Secure by Design**: Every endpoint protected by Amazon API Gateway with military-grade IAM security
- **🚀 DevOps Excellence**: Automated CI/CD pipeline ensures flawless deployments with zero downtime

## 🧭 Architecture: The "Central Hub"

Our API is the mission control center that connects users to the entire tender intelligence ecosystem! 🌟

```
React Frontend (AWS Amplify + Cognito) 
    ↓ [IAM Signed Requests]
Amazon API Gateway (IAM Auth)
    ↓ [Proxies Securely]
🧠 Lambda: Core Logic API (This Project) 
    ├─ 📊 Reads/Writes → Amazon RDS (MS SQL Server)
    ├─ 📧 Invokes → Lambda: Mailer Function
    ├─ 📋 Invokes → Lambda: Logging Function
    └─ 📈 Invokes → Lambda: Analytics Function
```

**🎯 The Intelligence Flow:**
- Users authenticate through Cognito-powered frontend
- All requests flow through our secure API Gateway fortress
- Our .NET 8 Lambda processes business logic with lightning speed
- Data flows seamlessly between our RDS database and supporting microservices

## 🧠 Core Responsibilities

This powerhouse is organized as a modern ASP.NET Core Web API with specialized controllers for maximum efficiency:

### 📊 **TendersController** - The Opportunity Engine
- `GET /api/tenders` 🔍 Delivers paginated lists of live opportunities
- `GET /api/tenders/{id}` 📋 Provides comprehensive tender details and specifications
- `GET /api/tenders/search` 🎯 Powers intelligent search with keywords, tags, sources, and advanced filters

### ⭐ **WatchlistController** - Personal Intelligence Hub
- `GET /api/watchlist` 👀 Retrieves user's curated opportunity portfolio
- `POST /api/watchlist/{tenderId}` ➕ Adds high-value opportunities to personal tracker
- `DELETE /api/watchlist/{tenderId}` 🗑️ Removes opportunities from surveillance

### 👤 **UserController** - Profile Command Center
- `GET /api/user/profile` 🆔 Fetches comprehensive user profile and preferences
- `PUT /api/user/profile` ✏️ Updates user information and notification settings

### 🦸 **AdminController** - Super User Mission Control
- `GET /api/admin/logs` 📋 Triggers advanced logging intelligence via Logging Function
- `GET /api/admin/analytics` 📈 Activates comprehensive system analytics via Analytics Function

### 📧 **NotificationService** - Communication Intelligence
Internal orchestration service that triggers the Mailer Function for:
- New opportunities matching user watchlists
- Deadline proximity alerts
- System notifications and updates

## 🧩 Project Structure

```
Tender_Tool_Core_Logic/
├── Controllers/
│   ├── TendersController.cs    # 🎯 Opportunity discovery & search engine
│   ├── WatchlistController.cs  # ⭐ Personal portfolio management
│   ├── UserController.cs       # 👤 Profile & preferences hub
│   └── AdminController.cs      # 🦸 Super user command center
├── Data/
│   └── ApplicationDbContext.cs # 🗄️ Master EF Core database context
├── Models/
│   ├── DTOs/                   # 📦 API response objects
│   │   ├── TenderDetailsDto.cs # 📋 Comprehensive tender information
│   │   ├── UserProfileDto.cs   # 👤 User profile data structures
│   │   └── SearchResultDto.cs  # 🔍 Search response formatting
│   └── Output/                 # 🗄️ EF Core database entities
│       ├── BaseTender.cs       # 📊 Core tender structure
│       ├── Tag.cs              # 🏷️ AI-generated categorization
│       └── UserWatchlist.cs    # ⭐ Personal opportunity tracking
├── Services/
│   ├── TenderSearchService.cs  # 🔍 Advanced search & filtering logic
│   ├── WatchlistService.cs     # ⭐ Personal portfolio operations
│   └── NotificationService.cs  # 📧 Communication orchestration
├── Program.cs                  # 🚀 Application bootstrap & DI setup
├── aws-lambda-tools-defaults.json # ⚙️ Deployment configuration
└── README.md                   # 📖 This comprehensive guide
```

## 📦 Tech Stack

- **🏗️ Runtime**: .NET 8 (LTS) - Latest long-term support for maximum stability
- **🌐 Framework**: ASP.NET Core Web API - Enterprise-grade web framework
- **☁️ Compute**: AWS Lambda - Serverless scalability and cost efficiency
- **🔌 API**: Amazon API Gateway (REST API) - Secure, managed API layer
- **🔒 Security**: AWS IAM Authorization - Military-grade request signing
- **🗄️ Database**: Amazon RDS (MS SQL Server) - Managed, scalable data storage
- **📊 Data Access**: Entity Framework Core 8 - Modern ORM with LINQ support
- **🚀 Deployment**: GitHub Actions - Automated CI/CD pipeline excellence

## ⚙️ Configuration (Critical)

### 🔧 Environment Variables

| Variable Name | Required | Description | Example Value |
|---------------|----------|-------------|---------------|
| `DB_CONNECTION_STRING` | ✅ Yes | SQL Server connection to RDS database | `Server=tender-db.cluster-xxx.rds.amazonaws.com;Database=TenderTool;...` |
| `MAILER_FUNCTION_ARN` | ✅ Yes | ARN for email notification service | `arn:aws:lambda:us-east-1:123456789:function:TenderMailer` |
| `LOGGING_FUNCTION_ARN` | ✅ Yes | ARN for advanced logging service | `arn:aws:lambda:us-east-1:123456789:function:TenderLogger` |
| `ANALYTICS_FUNCTION_ARN` | ✅ Yes | ARN for business intelligence service | `arn:aws:lambda:us-east-1:123456789:function:TenderAnalytics` |

> 💡 **Pro Tip**: Store sensitive configurations in AWS Secrets Manager for maximum security!

## 🔒 Security: API Gateway & IAM

Our security architecture is built like a digital fortress! 🏰

### 🛡️ **Multi-Layer Security Process:**

1. **🔐 Authentication**: Users authenticate via AWS Cognito in the React/Amplify frontend
2. **✍️ Request Signing**: Amplify automatically signs all API requests with AWS IAM signatures
3. **🔍 Gateway Validation**: API Gateway validates IAM signatures before processing
4. **✅ Trusted Execution**: Lambda receives only verified, authenticated requests
5. **👤 User Context**: User details flow securely through request context

**🎯 Security Benefits:**
- Zero exposed API keys or passwords
- Automatic request authentication
- Industry-standard AWS IAM security
- Complete audit trail of all requests

## 🗄️ Database: EF Core & Migrations

We are the **database schema owners** and migration masters! 🏗️

### 📋 **Migration Management:**

```bash
# Create new migration
dotnet ef migrations add FeatureName --context ApplicationDbContext

# Apply migrations to database
dotnet ef database update --context ApplicationDbContext

# Generate SQL scripts for production
dotnet ef migrations script --context ApplicationDbContext
```

### 🔒 **Security Best Practices:**
- Lambda IAM role has **read/write only** permissions (no DDL)
- Database connection uses dedicated service account
- Migrations applied through secure bastion or local development
- Production deployments use pre-generated SQL scripts

## 🚀 Getting Started (Local Development)

Ready to dive into the central command? Let's power up your development environment! 🔥

### 📋 Prerequisites
- .NET 8 SDK 🛠️
- AWS CLI configured with appropriate credentials 🔑
- Access to development database connection string 🗄️
- Your favorite IDE (Visual Studio 2022 recommended) 💻

### 🔧 Local Setup

1. **📁 Clone & Navigate**
   ```bash
   git clone <repository-url>
   cd Tender_Tool_Core_Logic
   ```

2. **📦 Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **🔐 Configure Secrets**
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-dev-db-connection"
   dotnet user-secrets set "MailerFunctionArn" "your-dev-mailer-arn"
   dotnet user-secrets set "LoggingFunctionArn" "your-dev-logging-arn"
   dotnet user-secrets set "AnalyticsFunctionArn" "your-dev-analytics-arn"
   ```

4. **🚀 Launch Development Server**
   ```bash
   dotnet watch run
   ```

5. **🧪 Test Your Setup**
   Navigate to `https://localhost:7001/swagger` for interactive API documentation!

### 🔍 **Local Testing Tips:**
- Temporarily disable `[Authorize]` attributes for local testing
- Use Postman with AWS Signature v4 for realistic request signing
- Monitor logs in console for debugging information

## 📦 Deployment

This ASP.NET Core Lambda function can be deployed using three different methods. Choose the one that best fits your workflow and requirements.

### Prerequisites

Before deploying, ensure you have:

- .NET 8 SDK installed
- AWS CLI configured with appropriate credentials
- SQL Server RDS instance running and accessible
- Required environment variables configured (see Configuration section)
- IAM permissions for Lambda execution and API Gateway

---

### Method 1: AWS Toolkit Deployment

Deploy directly from your IDE using the AWS Toolkit extension.

#### For Visual Studio 2022:

1. **Install AWS Toolkit:**
   - Install the AWS Toolkit for Visual Studio from the Visual Studio Marketplace

2. **Configure AWS Credentials:**
   - Ensure your AWS credentials are configured in Visual Studio
   - Go to View → AWS Explorer and configure your profile

3. **Deploy the Function:**
   - Right-click on the `Tender-Core-Logic.csproj` project
   - Select "Publish to AWS Lambda..."
   - Choose "ASP.NET Core Web API" as the function blueprint
   - Configure the deployment settings:
     - **Function Name**: `TenderCoreLogicAPI`
     - **Runtime**: `.NET 8`
     - **Memory**: `512 MB`
     - **Timeout**: `30 seconds`
     - **Handler**: `Tender-Core-Logic::Tender_Core_Logic.LambdaEntryPoint::FunctionHandlerAsync`

4. **Configure API Gateway:**
   - The function will automatically create an API Gateway with `/{proxy+}` and `/` routes
   - Both `ANY` and `OPTIONS` methods will be configured for CORS support
   - Note the generated API Gateway URL for testing

5. **Set Environment Variables:**
   ```
   DB_CONNECTION_STRING: Server=your-rds-endpoint,1433;Database=TenderTool;User Id=admin;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=True
   MAILER_FUNCTION_ARN: arn:aws:lambda:us-east-1:123456789:function:TenderMailer
   LOGGING_FUNCTION_ARN: arn:aws:lambda:us-east-1:123456789:function:TenderLogger
   ANALYTICS_FUNCTION_ARN: arn:aws:lambda:us-east-1:123456789:function:TenderAnalytics
   ```

#### For VS Code:

1. **Install AWS Toolkit:**
   - Install the AWS Toolkit extension for VS Code

2. **Open Command Palette:**
   - Press `Ctrl+Shift+P` (Windows/Linux) or `Cmd+Shift+P` (Mac)
   - Type "AWS: Deploy SAM Application"

3. **Follow the deployment wizard** to configure and deploy your function

---

### Method 2: SAM Deployment

Deploy using AWS SAM CLI with the provided serverless template.

#### Step 1: Install SAM CLI

```bash
# For Windows (using Chocolatey)
choco install aws-sam-cli

# For macOS (using Homebrew)
brew install aws-sam-cli

# For Linux (using pip)
pip install aws-sam-cli
```

#### Step 2: Install Lambda Tools

```bash
dotnet tool install -g Amazon.Lambda.Tools
```

#### Step 3: Navigate to Project Directory

```bash
cd Tender-Core-Logic/src/Tender-Core-Logic
```

#### Step 4: Configure Environment Variables

Create a `parameters.json` file for environment variables:

```json
{
  "DBConnectionString": "Server=your-rds-endpoint,1433;Database=TenderTool;User Id=admin;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=True",
  "MailerFunctionArn": "arn:aws:lambda:us-east-1:123456789:function:TenderMailer",
  "LoggingFunctionArn": "arn:aws:lambda:us-east-1:123456789:function:TenderLogger",
  "AnalyticsFunctionArn": "arn:aws:lambda:us-east-1:123456789:function:TenderAnalytics"
}
```

#### Step 5: Build and Deploy

```bash
# Build the project
dotnet restore
dotnet build -c Release

# Package the Lambda function (ASP.NET Core style)
dotnet lambda package -c Release -o ./lambda-package.zip Tender-Core-Logic.csproj

# Deploy using SAM with guided setup
sam deploy --template-file serverless.template \
           --stack-name tender-core-logic-api-stack \
           --capabilities CAPABILITY_IAM \
           --guided
```

#### Alternative: Direct SAM Deploy

For subsequent deployments after initial setup:

```bash
sam deploy --template-file serverless.template \
           --stack-name tender-core-logic-api-stack \
           --capabilities CAPABILITY_IAM \
           --parameter-overrides \
             DBConnectionString="Server=your-rds-endpoint,1433;Database=TenderTool;User Id=admin;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=True" \
             MailerFunctionArn="arn:aws:lambda:us-east-1:123456789:function:TenderMailer" \
             LoggingFunctionArn="arn:aws:lambda:us-east-1:123456789:function:TenderLogger" \
             AnalyticsFunctionArn="arn:aws:lambda:us-east-1:123456789:function:TenderAnalytics"
```

#### Important: IAM Policies

The serverless template includes `AWSLambda_FullAccess` policy. For production, consider using more restrictive permissions:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "lambda:InvokeFunction"
      ],
      "Resource": [
        "arn:aws:lambda:us-east-1:123456789:function:TenderMailer",
        "arn:aws:lambda:us-east-1:123456789:function:TenderLogger",
        "arn:aws:lambda:us-east-1:123456789:function:TenderAnalytics"
      ]
    },
    {
      "Effect": "Allow",
      "Action": [
        "logs:CreateLogGroup",
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Resource": "arn:aws:logs:*:*:*"
    }
  ]
}
```

---

### Method 3: Workflow Deployment (GitHub Actions)

Deploy automatically using GitHub Actions when pushing to the release branch.

#### Step 1: Set Up Repository Secrets

In your GitHub repository, go to Settings → Secrets and variables → Actions, and add:

```
AWS_ACCESS_KEY_ID: your-aws-access-key-id
AWS_SECRET_ACCESS_KEY: your-aws-secret-access-key
AWS_REGION: us-east-1
LAMBDA_FUNCTION_NAME: your-lambda-function-name
```

#### Step 2: Deploy via Release Branch

```bash
# Create and switch to release branch
git checkout -b release

# Make your changes and commit
git add .
git commit -m "Deploy Tender Core Logic API updates"

# Push to trigger deployment
git push origin release
```

#### Step 3: Monitor Deployment

1. Go to your repository's **Actions** tab
2. Monitor the "Deploy Lambda on Release Merge" workflow
3. Check the deployment logs for any issues

#### Manual Trigger

You can also trigger the deployment manually:

1. Go to the **Actions** tab in your repository
2. Select "Deploy Lambda on Release Merge"
3. Click "Run workflow"
4. Select the branch and click "Run workflow"

---

### Post-Deployment Verification

After deploying using any method, verify the deployment:

#### 1. Check Lambda Function

```bash
# Verify function exists and configuration
aws lambda get-function --function-name TenderCoreLogicAPI

# Check environment variables
aws lambda get-function-configuration --function-name TenderCoreLogicAPI
```

#### 2. Test API Gateway Endpoint

```bash
# Get the API Gateway URL from CloudFormation outputs
aws cloudformation describe-stacks --stack-name tender-core-logic-api-stack --query 'Stacks[0].Outputs'

# Test the health check endpoint
curl -X GET https://your-api-gateway-url.execute-api.us-east-1.amazonaws.com/Prod/

# Test API endpoints (requires proper IAM signing)
curl -X GET https://your-api-gateway-url.execute-api.us-east-1.amazonaws.com/Prod/api/tenders \
  --aws-sigv4 "aws:amz:us-east-1:execute-api" \
  --user "your-access-key:your-secret-key"
```

#### 3. Verify Database Connectivity

```bash
# Check CloudWatch logs for any connection issues
aws logs describe-log-groups --log-group-name-prefix "/aws/lambda/TenderCoreLogicAPI"

# View recent logs
aws logs tail "/aws/lambda/TenderCoreLogicAPI" --follow
```

---

### Environment Variables Setup

Configure these environment variables in your Lambda function:

| Variable | Value | Description |
|----------|-------|-------------|
| `DB_CONNECTION_STRING` | `Server=your-rds-endpoint,1433;Database=TenderTool;User Id=admin;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=True` | SQL Server connection string |
| `MAILER_FUNCTION_ARN` | `arn:aws:lambda:us-east-1:123456789:function:TenderMailer` | ARN for email notification service |
| `LOGGING_FUNCTION_ARN` | `arn:aws:lambda:us-east-1:123456789:function:TenderLogger` | ARN for advanced logging service |
| `ANALYTICS_FUNCTION_ARN` | `arn:aws:lambda:us-east-1:123456789:function:TenderAnalytics` | ARN for business intelligence service |

> **Security Note**: For production deployments, store sensitive configuration values in AWS Secrets Manager or Parameter Store instead of environment variables.

---

### API Gateway Configuration

The deployment automatically creates an API Gateway with the following configuration:

- **Base URL**: `https://{api-id}.execute-api.us-east-1.amazonaws.com/Prod/`
- **API Endpoints**:
  - `GET/POST/PUT/DELETE /api/tenders` - Tender management
  - `GET/POST/DELETE /api/watchlist` - Watchlist operations
  - `GET/PUT /api/user/profile` - User profile management
  - `GET /api/admin/logs` - Admin logging (super users only)
  - `GET /api/admin/analytics` - Admin analytics (super users only)

#### CORS Configuration:

The template includes `OPTIONS` methods for CORS support. Ensure your frontend is configured to handle CORS properly:

```javascript
// Example frontend configuration
const API_CONFIG = {
  endpoints: {
    apiGateway: 'https://your-api-gateway-url.execute-api.us-east-1.amazonaws.com/Prod'
  },
  aws_api_gateway_method: 'ANY',
  aws_api_gateway_path: '/{proxy+}'
};
```

---

### Database Migration Management

Since this is the central hub, you may need to run database migrations:

#### Development Migrations:

```bash
# Navigate to project directory
cd Tender-Core-Logic/src/Tender-Core-Logic

# Add new migration
dotnet ef migrations add YourMigrationName --context ApplicationDbContext

# Update database (development only)
dotnet ef database update --context ApplicationDbContext
```

#### Production Migrations:

```bash
# Generate SQL script for production
dotnet ef migrations script --context ApplicationDbContext -o migration.sql

# Apply manually to production database via secure connection
```

---

### IAM Security Configuration

For production deployment, ensure proper IAM policies:

#### Lambda Execution Role:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "logs:CreateLogGroup",
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Resource": "arn:aws:logs:*:*:*"
    },
    {
      "Effect": "Allow",
      "Action": [
        "lambda:InvokeFunction"
      ],
      "Resource": [
        "arn:aws:lambda:*:*:function:TenderMailer",
        "arn:aws:lambda:*:*:function:TenderLogger",
        "arn:aws:lambda:*:*:function:TenderAnalytics"
      ]
    }
  ]
}
```

#### API Gateway Resource Policy (for IAM authentication):

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": "*",
      "Action": "execute-api:Invoke",
      "Resource": "arn:aws:execute-api:us-east-1:123456789:*/*/api/*",
      "Condition": {
        "StringEquals": {
          "aws:SourceIp": "your-allowed-ip-ranges"
        }
      }
    }
  ]
}
```

---

### Troubleshooting Deployment Issues

**Build Failures:**
- Ensure .NET 8 SDK is installed and available in PATH
- Verify all NuGet packages are restored successfully
- Check for any compilation errors in the project

**Deployment Permission Errors:**
- Verify AWS credentials have necessary permissions for Lambda, API Gateway, and IAM
- Check CloudFormation stack creation permissions
- Ensure S3 bucket access for deployment artifacts

**Runtime Errors:**
- Check CloudWatch logs for detailed error messages
- Verify environment variables are correctly set
- Test database connectivity from Lambda VPC
- Validate IAM permissions for invoking other Lambda functions

**API Gateway Integration Issues:**
- Verify proxy integration is correctly configured
- Check CORS settings if accessing from web frontend
- Ensure proper IAM authentication is configured
- Test API endpoints with proper AWS Signature v4 signing

**Function Update Failures:**
- Verify the function name matches between secrets and actual function
- Check that the ZIP file is properly formatted
- Ensure the function handler path is correct
- Validate Lambda runtime and architecture settings
## 🧰 Troubleshooting & Team Gotchas

### 🚨 Common Mission-Critical Issues

<details>
<summary><strong>🚫 ERROR: 403 Forbidden on All Requests</strong></summary>

**Issue**: Every API call returns 403 Forbidden status.

**Root Cause**: IAM request signing is failing or missing required permissions.

**🔧 Fix Checklist:**
- ✅ Verify Amplify app's authenticated role has `execute-api:Invoke` permissions
- ✅ Check API Gateway ARN in IAM policy matches deployed API
- ✅ Ensure requests are properly signed with AWS Signature v4
- ✅ Validate user authentication status in Cognito

</details>

<details>
<summary><strong>⏰ ERROR: 504 Gateway Timeout / Task Timed Out</strong></summary>

**Issue**: API calls timing out after 30 seconds.

**Root Cause**: VPC networking issues preventing database connectivity.

**🔧 Network Diagnosis:**
1. **Lambda Security Group**: Outbound rule for port 1433 to RDS
2. **RDS Security Group**: Inbound rule from Lambda security group
3. **VPC Configuration**: Lambda and RDS in same VPC/subnets
4. **Route Tables**: Proper routing between Lambda and RDS subnets

</details>

<details>
<summary><strong>🗄️ ERROR: Database Connection Failures</strong></summary>

**Issue**: EF Core cannot connect to SQL Server database.

**Root Cause**: Connection string or VPC configuration issues.

**🔧 Database Troubleshooting:**
- ✅ Verify connection string format and credentials
- ✅ Test database connectivity from VPC bastion host
- ✅ Check RDS instance status and security groups
- ✅ Validate SQL Server authentication mode

</details>

<details>
<summary><strong>🔄 ERROR: Microservice Invocation Failures</strong></summary>

**Issue**: Cannot invoke Mailer, Analytics, or Logging functions.

**Root Cause**: IAM permissions or function ARN configuration issues.

**🔧 Service Integration Fix:**
- ✅ Verify Lambda execution role has `lambda:InvokeFunction` permissions
- ✅ Check function ARNs in environment variables
- ✅ Test function invocation from AWS Console
- ✅ Review CloudWatch logs for detailed error messages

</details>

---

> Built with love, bread, and code by **Bread Corporation** 🦆❤️💻
