# DVLD — Driver Vehicle License Department

A Windows desktop application for managing driving-license department workflows. The project is built with **C#**, **Windows Forms (WinForms)**, and **Microsoft SQL Server**.

Repository: https://github.com/Muhannad-Alrashed/DVLD

## Overview

DVLD is structured as a multi-layer desktop application, separating the user interface, business logic, and data-access code into distinct projects. This separation helps keep UI concerns, application rules, and database operations organized.

## Technology Stack

- **Language:** C#
- **Desktop UI:** Windows Forms (WinForms)
- **Database:** Microsoft SQL Server
- **Architecture:** UI, business-logic, and data-access layers

## Solution Structure

The repository is organized into the following main directories:

```text
DVLD/
├── DVLD/             # WinForms desktop application / presentation layer
├── DVLD_Business/    # Business logic and application operations
├── DVLD_DataAccess/  # Data access and database operations
├── .gitattributes
├── .gitignore
└── README.md
```

The exact classes and responsibilities within each project can be explored in the corresponding source files.

## Prerequisites

Before opening or running the application, prepare a Windows development environment with:

- Visual Studio with the **.NET desktop development** workload
- A .NET Framework or .NET runtime version compatible with the project files
- Microsoft SQL Server
- SQL Server Management Studio (SSMS), or another SQL client, for database administration

Check the solution and project files to identify the exact Visual Studio version and target framework required by this version of the project.

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/Muhannad-Alrashed/DVLD.git
cd DVLD
```

### 2. Open the solution

Open the solution file (`.sln`) in Visual Studio. If the solution file is not at the repository root, locate it in the project folders.

Allow Visual Studio to restore any required NuGet packages.

### 3. Configure the database

Make sure SQL Server is installed and running. Inspect the data-access project and application configuration files to determine:

- The expected database name and schema
- Whether a database creation script or backup is included
- The SQL Server instance and authentication method
- The connection-string key used by the application

If a database script or backup is provided in the repository, initialize the database using the project's instructions. Update the connection string in the appropriate configuration file for your local SQL Server instance. Do not commit credentials or other secrets.

### 4. Build and run

1. In Visual Studio, select the appropriate startup project (normally the WinForms application project).
2. Confirm that the target framework and database configuration are correct.
3. Build the solution.
4. Start the application using **Start** or **F5**.

Successful startup may depend on having the expected database schema and data configured first.

## Architecture

The repository's top-level projects indicate a layered design:

- **Presentation layer (`DVLD/`)** — Windows Forms screens and user interaction.
- **Business layer (`DVLD_Business/`)** — application and domain operations.
- **Data-access layer (`DVLD_DataAccess/`)** — database communication.

This organization separates the desktop interface from business rules and persistence concerns.

## Troubleshooting

- **The solution does not load:** Check that Visual Studio supports the target framework specified in the project files.
- **Build errors or missing packages:** Restore NuGet packages and review the build output for the specific dependency or framework issue.
- **SQL Server connection failures:** Verify that the SQL Server service is running and that the server/instance, authentication mode, and connection string are correct.
- **Missing tables or database objects:** Confirm that the correct database setup script or backup has been applied.
- **The application opens but cannot load data:** Check the database connection and whether the database contains the records expected by the application.

## Contributing

Suggestions, bug reports, and improvements are welcome.

1. Fork the repository.
2. Create a branch for your change.
3. Make and test your changes.
4. Commit with a clear message.
5. Open a pull request describing the change.

For bug reports or feature requests, use the [GitHub repository](https://github.com/Muhannad-Alrashed/DVLD).

## License

Check the repository for a `LICENSE` file to confirm the terms under which the project may be used, modified, or redistributed.
