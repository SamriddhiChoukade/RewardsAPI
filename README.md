# RewardsAPI

A beginner-friendly .NET Web API project for managing rewards.  
This README will guide you step-by-step on how to clone, setup, run, and test the API.

---

## Table of Contents
1. [Tech Stack](#tech-stack)
2. [Prerequisites](#prerequisites)
3. [Clone the Repository](#clone-the-repository)
4. [Setup & Run](#setup--run)
5. [API Endpoints](#api-endpoints)
6. [Testing the API](#testing-the-api)


---

## Project Overview
This project is a RESTful API for a rewards program, handling:
- Member registration & OTP verification
- Points management (add/view points)
- Optional coupon redemption
- JWT-based authentication for protected endpoints


---


## Tech Stack
- **Backend:** .NET 8 (ASP.NET Core Web API)  
- **Database:** PostgreSQL/MySQL  
- **ORM:** Entity Framework Core  
- **Authentication:** JWT  
- **Frontend (Optional):** HTML + CSS + Vanilla JS

---

## Prerequisites
Before you start, make sure you have installed:  
1. **.NET SDK**: [Download here](https://dotnet.microsoft.com/download)  
2. **Git**: [Download here](https://git-scm.com/downloads)  
3. Optional: **Postman** (for API testing)  

Check installations in terminal:  
```bash
dotnet --version
git --version
