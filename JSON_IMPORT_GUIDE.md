# JSON Import Guide for DnD Adventure

This guide explains how to import JSON files into your DnD Adventure application using the Docker Compose setup.

## Overview

The DnD Adventure application now supports importing game worlds from JSON files through multiple methods:

1. **Direct JSON Content Import** - Import JSON content directly via API
2. **File Upload Import** - Upload JSON files through the API
3. **JSON Validation** - Validate JSON structure before importing

## API Endpoints

### 1. Validate JSON Content
**Endpoint:** `POST /api/save/validate-json`

Validates JSON content without importing it into the current world.

```http
POST http://localhost:5000/api/save/validate-json
Content-Type: application/json

{
  "jsonContent": "{ ... your JSON content ... }"
}
```

**Response:**
```json
{
  "isValid": true,
  "worldName": "Realm of the Crystal Spire",
  "characterCount": 1,
  "mapCount": 1,
  "npcCount": 1,
  "message": "JSON is valid and ready for import"
}
```

### 2. Import JSON Content
**Endpoint:** `POST /api/save/import`

Imports JSON content directly and loads it as the current world.

```http
POST http://localhost:5000/api/save/import
Content-Type: application/json

{
  "jsonContent": "{ ... your JSON content ... }"
}
```

**Response:** Returns the imported World object

### 3. Import JSON File
**Endpoint:** `POST /api/save/import-file`

Uploads and imports a JSON file.

```http
POST http://localhost:5000/api/save/import-file
Content-Type: multipart/form-data

file: [your .json file]
```

**Response:** Returns the imported World object

### 4. Get Available Saves
**Endpoint:** `GET /api/save`

Lists all available save files including imported worlds.

```http
GET http://localhost:5000/api/save
```

### 5. Create Quick Save
**Endpoint:** `POST /api/save/quicksave`

Creates a quick save of the currently loaded world.

```http
POST http://localhost:5000/api/save/quicksave
```

## JSON Structure

The application expects JSON files to follow this structure:

```json
{
  "character": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "Character Name",
    "race": "Elf",
    "class": "Mage",
    "level": 1,
    "healthPoints": 8,
    "maxHealthPoints": 8,
    "attributes": {
      "Strength": 10,
      "Dexterity": 12,
      "Constitution": 10,
      "Intelligence": 16,
      "Wisdom": 14,
      "Charisma": 12
    },
    "inventory": ["Item1", "Item2"]
  },
  "world": {
    "id": "7b8e8000-c2a1-4c61-b556-447755550000",
    "name": "World Name",
    "description": "World description",
    "createdAt": "2024-03-20T10:30:00Z",
    "lastSavedAt": "2024-03-20T10:30:00Z",
    "locations": {
      "Location1": "Description1",
      "Location2": "Description2"
    },
    "quests": {
      "quest-id": "Quest description"
    },
    "maps": {
      "map-id": {
        "id": "map-id",
        "name": "Map Name",
        "description": "Map description",
        "grid": [...]
      }
    },
    "npcs": {
      "npc-id": {
        "id": "npc-id",
        "name": "NPC Name",
        "race": "Human",
        "occupation": "Merchant",
        "description": "NPC description"
      }
    }
  }
}
```

## Docker Compose Usage

### Starting the Application

1. **Build the containers:**
   ```bash
   docker-compose build
   ```

2. **Start the services:**
   ```bash
   docker-compose up
   ```

3. **Access the API:**
   - API: http://localhost:5000
   - Web: http://localhost:5001

### Testing JSON Import

1. **Use the provided test file:**
   The repository includes a `Test.json` file with sample data.

2. **Use the HTTP test file:**
   Open `test-json-import.http` in VS Code with the REST Client extension to test the endpoints.

3. **Manual testing with curl:**
   ```bash
   # Validate JSON
   curl -X POST http://localhost:5000/api/save/validate-json \
     -H "Content-Type: application/json" \
     -d '{"jsonContent": "..."}'

   # Import JSON
   curl -X POST http://localhost:5000/api/save/import \
     -H "Content-Type: application/json" \
     -d '{"jsonContent": "..."}'

   # Upload file
   curl -X POST http://localhost:5000/api/save/import-file \
     -F "file=@Test.json"
   ```

## Development Container

For development work, you can use the dev container:

```bash
docker-compose run --rm dev bash
```

This gives you access to the .NET SDK and all source code for development and testing.

## Error Handling

The import functionality includes comprehensive error handling:

- **Invalid JSON:** Returns validation errors with specific messages
- **Missing required fields:** Identifies missing or malformed data
- **File format errors:** Ensures only .json files are accepted
- **Serialization errors:** Provides detailed error messages for debugging

## File Storage

- **Save Directory:** `Saves/` (created automatically)
- **Imported worlds:** Saved as timestamped JSON files
- **Quick saves:** Automatically named with world name and timestamp

## Supported Features

The JSON import system supports all core game features:

- ✅ Characters with attributes, inventory, and equipment
- ✅ World data with locations and quests
- ✅ Maps with grid-based layouts
- ✅ NPCs with dialog systems
- ✅ Points of Interest and structures
- ✅ Game states and progress tracking
- ✅ Equipment and character progression

## Troubleshooting

### Common Issues

1. **Port conflicts:** Ensure ports 5000 and 5001 are available
2. **JSON validation errors:** Check JSON syntax and required fields
3. **File upload issues:** Ensure file is valid JSON with .json extension
4. **Container build errors:** Run `docker-compose build --no-cache`

### Logs

View container logs for debugging:
```bash
docker-compose logs api
docker-compose logs web
```

## Next Steps

After importing a JSON world:

1. **Explore the world:** Use the game API to interact with characters and locations
2. **Save progress:** Create saves as you play
3. **Export worlds:** Use the save functionality to export your progress
4. **Modify data:** Edit JSON files to customize your game world

For more information, see the main README.md file and API documentation.
