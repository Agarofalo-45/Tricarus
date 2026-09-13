# Tricarus

Welcome to the **Tricarus** project! This document outlines how to set up the project locally and best practices for collaborating and working with Unity and Git.

## Prerequisites

Before you begin, ensure you have the following installed:
1. **Git** and **Git LFS** (Large File Storage). 
   - *Note: This project uses Git LFS for binary files like textures, models, and audio.*
2. **Unity Hub**
3. **Unity 6000.5.6f1** (Install this specific version via Unity Hub).

## Local Setup

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   ```
2. **Initialize Git LFS (if you haven't globally):**
   Navigate to the project folder and run:
   ```bash
   cd Tricarus
   git lfs install
   git lfs pull
   ```
   *(This ensures all large files are downloaded correctly instead of placeholder pointer files).*

3. **Open the project in Unity:**
   - Open **Unity Hub**.
   - Click **Add** -> **Add project from disk**.
   - Select the `Tricarus` repository folder.
   - Click on the project in Unity Hub to open it. *(The first launch may take some time as Unity imports assets and builds the Library folder).*

## Working with Changes (Importing & Exporting)

When collaborating on this Unity project, follow these guidelines to avoid merge conflicts and missing references:

### Branching & Committing
- Always create a new branch for your feature or bug fix:
  ```bash
  git checkout -b feature/your-feature-name
  ```
- Before starting work, make sure your local branch is up to date: `git pull`.
- When you are ready to share your changes, stage and commit them, then push your branch and open a Pull Request.

### Unity & Git Best Practices
- **Never commit the `Library/`, `Temp/`, `Obj/`, `Build/`, or `Logs/` folders.** (These should be ignored by the project's `.gitignore`).
- **Always commit `.meta` files!** Every asset in Unity (scripts, textures, models) has an associated `.meta` file. If you add, move, or rename an asset, ensure the `.meta` file is committed alongside it. Failing to do so will break references in scenes and prefabs.
- **Save frequently.** Use `File > Save` for scenes and `File > Save Project` to flush any unsaved asset changes to disk before you commit in Git.
- **Coordinate on Scenes & Prefabs.** Unity Scene (`.unity`) and Prefab (`.prefab`) files can be difficult to merge if multiple people edit them simultaneously. If you are going to modify a core scene or a complex prefab, coordinate with the team to avoid conflicts.

### Pulling Updates (Importing Changes)
If teammates have made changes to the repository, you can bring them into your local environment:
1. Make sure you have saved your work in Unity.
2. Pull the latest changes from Git:
   ```bash
   git pull
   ```
3. If new binary files were added, they should download automatically. If you notice broken assets (or files that show up as 1KB text pointers instead of actual models/textures), run:
   ```bash
   git lfs pull
   ```
4. Return to Unity. It will detect the file changes on disk and automatically re-import the updated assets.
