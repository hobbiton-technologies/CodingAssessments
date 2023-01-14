# CodingAssessments

This repository contains a coding assessment designed to test your programming skills and knowledge.

# FRONTEND


Create a simple Travel Insurance application  written in either Angular or React The application should have the following features:
1. Show a list of travel insurance packages with their benefits
2. A form for adding packages (should include the name, description, premium, and list of benefits)
3. Ability to edit and delete added insurance packages

#### Submission
1. Code for the application
2. A README with instructions on how to run the code
3. [Optional] A link to the deployed application


#### Resources

  * [API Docs](https://assessment.hobbiton.tech/)
  * [Angular](https://angular.io/)
  * [React](https://reactjs.org/)


# BACKEND

## QUESTION 1(TRAVEL INSURANCE APP)


#### Objective:

Create a RESTful API for a travel insurance package that connects to a database. The package model should include a list of benefits.

The API should allow users to view, create, update, and delete travel insurance packages and their associated benefits.

Implement an endpoint to upload a file for the supportingDocument.

#### Requirements:

* Use a web framework of your choice (e.g. Express, NEST JS, ASP.NET)

* Connect to a database (e.g. PostgreSQL, MySQL, SQLite) to store and retrieve data for the travel insurance packages and their associated benefits.

The package object should have the following fields:

```id: a unique identifier for the package (integer)

name: the name of the package (string)

description: a brief description of the package (string)

premium: the cost of the package (float)

supportingDocumentUrl: a URL to any supporting documents for the package (string)

benefits: an array of benefits associated with the package, where each benefit has the following fields:

   id: a unique identifier for the benefit (integer)

   name: the name of the benefit (string)

   description: a brief description of the benefit (string)
   ```



## QUESTION 2 (REPORT GENERATION)

#### Objective

Create a backend service using a web framework (Express, NestJS, or ASP.NET) that generates CSV files for the following reports:

#### Requirements

1. Summary report: summary of all transactions made by the user including total deposits and total withdraws.
2. Detailed report: breakdown of all transactions made by the user including date, and amount of each transaction.
3. Balance report: breakdown of the user's current balance including total deposits and total withdraws.
4. Summary report: a report that shows the total number of transactions made per day,week,month,year.
5. Summary report: a report that shows the total number of transactions made by the user per day,week,month,year.

#### Transactions Table Schema:
table: transactions
```id (integer)
user_id (integer)
type (string) (deposit or withdraw)
amount (float)
date (datetime)
```

#### Users Table Schema:
table: users
```id (integer)
username (string)
email (string)
```

#### DB connection string
```Host=postgres.hobbiton.tech;Port=5432;Database=playground;Username=postgres;Password=d6308e9b751a0853```



#### Instructions:

1. Connect to a database and retrieve the transactions data for a specific user.
2. Create functions for the above mentioned reports
3. Write all the reports to a CSV file.
4. Create an endpoint to serve the generated reports in CSV format.


#### Submission:

1. Create a public repository on Github and push your code there.
2. Send the link to the repository along with a brief explanation of your design choices and any additional information you think is relevant.



#### Resources

* [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet/apis)
* [NestJs](https://nestjs.com/)
* [Express](https://expressjs.com/)


