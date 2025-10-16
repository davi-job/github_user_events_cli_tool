# Github Events CLI Tool

A command-line interface tool to fetch and display GitHub user events.

## How It Works

This tool interacts with the GitHub API to retrieve events associated with a specified GitHub user. It displays various types of events such as commits, issues, pull requests, and more in a user-friendly format.

### Key Features

-   Fetches user events from GitHub.
-   Displays event types including commits, issues, pull requests, and more.
-   Handles errors gracefully, providing feedback to the user.

## Tech Used

-   **C#**: The primary programming language used for development.
-   **.NET 9.0**: The framework used to build and run the application.
-   **GitHub API**: The API used to fetch user events.
-   **Json.NET**: For JSON serialization and deserialization.

## How to Run

1. **Clone the Repository**:

    ```bash
    git clone https://github.com/davi-job/github_user_events_cli_tool.git
    cd github_user_events_cli_tool
    ```

2. **Build the Project**:
   Use the following command to build the project:

    ```bash
    dotnet build
    ```

3. **Run the Application**:

    - To run the application without any arguments:
        ```bash
        dotnet run
        ```
    - To run the application with a specific GitHub username as an argument:
        ```bash
        dotnet run <username>
        ```
    - To package the application and install it as a global tool:
        ```bash
        dotnet pack
        dotnet tool install --global <package-name>
        ```

4. **Input Username**:
   When prompted, enter the GitHub username whose events you want to fetch.

5. **View Events**:
   The application will display the events associated with the specified user.

## Project URL

[Backend Roadmap Projects](https://roadmap.sh/projects/github-user-activity)
