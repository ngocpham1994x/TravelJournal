# Travel Journal Web Application
#### Video Demo:  <URL HERE>
#### Description:

## Overview

The final project is a **C# .NET MVC web application** created using Visual Studio IDE, designed to log travel locations that a person can visit throughout their lifetime. 
<br>
This application is containerized using **Docker Compose**, with two Docker images: 
- one for **ASP.NET**
- one for **SQL Server 2022**. 
<br>

The app consists of two primary tables (models): **Locations** and **Cities**, which have a one-to-many relationship. A traveler can log multiple locations within a specific city. Each table includes latitude and longitude properties to represent the geographical location of either the city or the specific location visited.

## Table Models

### 1. Locations Table
- Contains 8 properties:
  - **Place Name**
  - **Address**
  - **City Name**
  - **Country Name**
  - **Latitude**
  - **Longitude**
  - **Date of Visit**
  - **Time of Visit**
- **City Name** and **Country Name** are derived from the **Cities** table via the one-to-many relationship. 
- The primary key for this table is the **ID**.

### 2. Cities Table
- Contains 4 properties:
  - **City Name**
  - **Country Name**
  - **Latitude**
  - **Longitude**
- The primary key is a composite of the **City Name** and **Country Name**.

## Application Features

### CRUD Operations

The application supports basic **CRUD (Create, Read, Update, Delete)** operations, with corresponding views created using *cshtml* files for each operation: **Index**, **Create**, **Edit**, **Details**, and **Delete**. Here is how the operations function:

#### Read
- The user accesses the **Location** page, which directs them to the main **Index** page displaying all logged locations, including each entry's details.

#### Create
- To add a new entry, the user clicks on the **Create Entry** link. A new page opens, prompting the user to fill in three essential fields for a location:
  - **Address**
  - **City**
  - **Country**
- These fields cannot be left empty, and an error message is displayed if any of them are missing. If the city is not already listed in the **Cities** table (case-insensitive check), the application automatically creates a new city entry. If the city exists, no new entry is created.
- Other fields such as **Place Name**, **Date of Visit**, **Time of Visit**, **Lat**, **Lon** are optional and can be left blank.
- In the **Locations** table, duplicate entries with the same **address**, **city**, and **country** cannot be created. Similarly, in the **Cities** table, duplicate entries with the same **city** and **country** are not allowed. If a user attempts to create such entries, an error message will be displayed in red on the page.


#### Update
- Users can edit any field of an existing **Location** entry. However, the **Address**, **City**, and **Country** fields cannot be set to null. If any mistakes are made during creation, these fields can be updated.
- A **Get Accurate Coordinates** button fetches the latitude and longitude values based on the **Address**, **City**, and **Country** via the **Google Maps API**. This allows the user to save the correct geographical coordinates for the location.

#### Delete
- Users can delete entries by clicking the **Delete** button on the **Delete** page. Deleting a **City** entry will also delete all associated **Location** entries. A confirmation prompt is displayed before deletion occurs.

#### Details
- The **Details** page provides a convenient view of the entry, displaying all the property values in a vertical layout. If latitude and longitude values exist, an embedded map is displayed using the **Google Maps API**.
- Additionally, the current temperature at the geographical coordinates is shown, fetched via the **OpenWeather API**.
- The page also includes an **Edit** button and a **Back to List** button.

## Additional Features

- **User Authentication**: Users can only create, edit, or delete entries after logging in. New users can register an account, though there is no email confirmation feature implemented yet.
  
- **Sorting**: Users can sort both the **Location** and **City** tables based on **City** and **Country** properties, in either ascending or descending order.

## Technologies and Learning

Through the development of this Travel Journal web application, I gained valuable knowledge in various areas of software development, particularly within the **.NET framework** and **C#**. I applied concepts learned in **CS50**, such as handling HTTP requests and responses, utilizing APIs, and establishing one-to-many relationships within **C#** and **Razor Pages**, also **CSS styling**. Additionally, I learned how to create secure API keys on **GitHub** to protect sensitive information, containerize the application using **Docker** and **Docker Compose**, and deploy the web application.

While I have learned a lot throughout this project, there is still much more I can improve and learn as I continue my journey as a new programmer.

---

Thank you for exploring my **Travel Journal web application**! Happy coding!