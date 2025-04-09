DnDAdventure Project Structure Summary
This project is a Dungeons & Dragons text adventure game built with .NET, consisting of a web-based frontend and a backend API. The application allows users to create characters and play through a text-based adventure where choices influence the story progression.

Project Architecture
The solution follows a layered architecture with the following components:

DnDAdventure.Core
Contains domain models and interfaces:
Character
 - Represents player characters with attributes, inventory, health points
GameState
 - Tracks player progress, location, quests, and story nodes
AdventureNode
 - Represents story segments with description and available choices
IRepository<T> - Generic repository pattern interface
IGameService - Service interface for game logic

DnDAdventure.Infrastructure
Implements services and repositories:
InMemoryRepository<T>
 - In-memory implementation of the repository pattern
GameService
 - Core game logic implementation including character/game state management

DnDAdventure.AI
Contains AI-driven adventure generation:
AdventureGenerator
 - Generates adventure nodes dynamically, with fallback content if AI is unavailable

DnDAdventure.API
RESTful API endpoints for game functionality:
GameController
 - Handles game state management, character creation, and choice processing

DnDAdventure.Web
Razor Pages frontend:
Character creation form
Adventure interface showing story text and choices
Character status panel

Key Features
Character Creation: Players can create characters with different races and classes
Dynamic Adventure Generation: Story nodes are generated using AI with fallback content
Choice-Based Gameplay: Players make choices that influence the story progression
State Persistence: Game state is maintained between requests

Technical Details
Built with .NET (appears to be .NET 7.0 based on Dockerfile)
Follows clean architecture principles with separation of concerns
Uses in-memory storage for repositories
Containerized with Docker for easy deployment
Frontend built with Razor Pages and Bootstrap
API communication handled with JavaScript fetch API

Project Setup
The solution can be created using the dotnet CLI commands shown in the AI.txt file, which creates the project structure and sets up project references.

Deployment
A Dockerfile is included for containerizing both the API and web application for deployment.