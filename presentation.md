# Technical Interview Presentation

## Project Overview

For this exercise I decided to implement a **Task Management System**.

Although the domain is intentionally simple, it provides enough
functionality to demonstrate the engineering practices requested in the
assignment, including:

-   Clean Architecture
-   Authentication and Authorization
-   CRUD operations
-   Business validation
-   Database persistence
-   Frontend integration
-   Automated testing
-   Responsible use of Generative AI

The objective was not to build the most feature-rich application, but
rather to build a maintainable, testable, and well-structured solution.

------------------------------------------------------------------------

# User Story

> **As a registered user, I want to manage my personal tasks so that I
> can organize my work and keep track of pending activities.**

This user story served as the foundation for the entire implementation.

From this single requirement I derived the application's business rules,
API endpoints, frontend functionality, and testing strategy.

------------------------------------------------------------------------

# Thought Process

Before writing any code, I identified the core concepts of the
application.

The domain consists of only two entities:

-   User
-   Task

A User owns multiple Tasks.

From that domain model I defined the business rules:

-   Users can register and authenticate.
-   Users can only access their own tasks.
-   Task titles are required.
-   Due dates cannot be in the past when creating a task.
-   Email addresses must be unique.
-   Completed tasks cannot return to previous states.

Keeping the business domain intentionally small allowed me to focus on
software architecture and engineering quality.

------------------------------------------------------------------------

# Architecture

The backend follows **Clean Architecture**.

The solution is divided into four projects:

-   Domain
-   Application
-   Infrastructure
-   API

Each layer has a single responsibility.

Business rules live in the Application layer while Infrastructure
contains technical concerns such as Entity Framework Core, SQLite, and
JWT authentication.

This keeps the business logic independent from frameworks and makes it
easier to test.

------------------------------------------------------------------------

# Frontend

The frontend was implemented using Angular.

Key design decisions include:

-   Standalone Components
-   Signals
-   Signal Forms
-   Lazy-loaded routes
-   Route Guards
-   JWT authentication
-   Responsive UI

The frontend communicates exclusively through the REST API.

------------------------------------------------------------------------

# Technical Decisions

Some important implementation decisions include:

-   SQLite for zero-configuration local setup.
-   Entity Framework Core as the ORM.
-   JWT Bearer authentication.
-   Swagger/OpenAPI documentation.
-   Seeded demo data for quick evaluation.
-   Monorepo structure with shared documentation.

The goal was to minimize setup complexity while still demonstrating
production-quality architecture.

------------------------------------------------------------------------

# AI-Assisted Development Workflow

One aspect I wanted to emphasize was the structured use of Generative AI
during development.

Instead of asking an AI assistant to generate the entire application in
a single prompt, I created documentation first so the AI could work with
consistent project context.

The project contains shared documentation such as:

-   Project Context
-   Business Rules
-   User Story
-   Domain Model
-   API Contract
-   Clean Architecture
-   Technical Decisions
-   Frontend Architecture
-   AI Instructions
-   Implementation Plan

This documentation acted as persistent context for the AI throughout
development.

By providing architecture, business rules, and implementation
constraints before generating code, the AI produced code that remained
consistent with the project's design.

AI was primarily used to accelerate implementation, while architectural
decisions, business rules, and final validation remained my
responsibility.

------------------------------------------------------------------------

# Testing Strategy

Testing was one of the primary focuses of this project.

Rather than relying only on unit tests, I implemented multiple testing
layers.

## Unit Tests

Unit tests validate:

-   Business rules
-   Validation
-   Authentication
-   Authorization
-   CRUD behavior
-   Error conditions

Business logic is tested independently from infrastructure whenever
possible.

## End-to-End Tests

I also implemented automated End-to-End tests using Playwright.

These tests validate complete user workflows, including:

-   User registration
-   Login
-   Route protection
-   Task creation
-   Task update
-   Task completion
-   Task deletion

Unlike unit tests, these execute the application as a real user would,
providing additional confidence that all layers work correctly together.

Having both unit tests and E2E tests provides confidence at both the
component and application levels.

------------------------------------------------------------------------

# Documentation

To reduce ambiguity and improve maintainability, I documented the
project before and during implementation.

Documentation includes:

-   User Story
-   Domain Model
-   Business Rules
-   API Contract
-   Clean Architecture
-   Technical Decisions
-   Frontend Architecture
-   README files
-   AI workflow documentation

This documentation also improved the quality and consistency of
AI-generated code.

------------------------------------------------------------------------

# Demonstration

The demonstration consists of the following flow:

1.  Register a new user.
2.  Log in.
3.  Access a protected route.
4.  View existing tasks.
5.  Create a new task.
6.  Update a task.
7.  Mark a task as completed.
8.  Delete a task.
9.  Demonstrate authorization by showing protected endpoints require
    authentication.

------------------------------------------------------------------------

# Future Improvements

If this project were extended beyond the interview exercise, I would
consider adding:

-   Refresh Tokens
-   Role-based authorization
-   Pagination and filtering
-   Soft deletes
-   Audit logging
-   Docker Compose
-   CI/CD pipeline
-   Integration tests
-   Cloud deployment

------------------------------------------------------------------------

# Final Thoughts

The goal of this project was not to build the largest application
possible.

Instead, I wanted to demonstrate software engineering practices that are
valuable in production systems:

-   Clean Architecture
-   SOLID principles
-   Separation of concerns
-   Testability
-   Maintainability
-   Automated testing
-   Responsible and structured use of Generative AI

I believe these practices are more important than domain complexity and
result in software that is easier to understand, extend, and maintain
over time.
