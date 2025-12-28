# Simple Deck

An audio control panel built with **.NET 10** and **WPF**, designed to interface with a custom **STM32-based** microcontroller platform.

## Table of Contents
* [Overview](#overview)
* [Tech Stack](#tech-stack)
* [Architecture](#architecture)
* [Getting Started](#getting-started)
* [Future Roadmap](#future-roadmap)

## Overview
Simple Deck Software serves as a bridge between high-level desktop interaction and low-level hardware control. The application provides a user-friendly interface to manage and interact with custom STM32 device, allowing user to control some aspects of audio, and app controls.

## Tech Stack
* **Framework:** .NET 10
* **Test Framework** XUnit
* **UI Layer:** Windows Presentation Foundation (WPF)
* **Hardware Interface:** STM32 Microcontroller Platform
* **Language:** C#

## Architecture
The project is currently structured as a modular desktop application. It emphasizes:
* **Hardware Communication:** Dedicated abstraction layers for interacting with the STM32 firmware.
* **Separation of Concerns:** Clear distinction between UI logic (WPF) and the underlying hardware communication protocols.

## Getting Started

### Installation

Currently, there is no installer or .exe file to run. Check Roadmap to know more about it

## Future Roadmap
 
- [x] Working Connection With SimpleDeck Hardware
- [x] Working Audio Module
- [ ] Fully working Config Module
- [ ] Working GUI
- [ ] 100% Test Coverage
- [ ] Fully Automatic Installer
- [ ] Auto-Update Function
- [ ] Add Logger