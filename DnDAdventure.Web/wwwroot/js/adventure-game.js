// Adventure Game JavaScript
class AdventureGame {
    constructor() {
        this.gameStateId = document.getElementById('game-state-id')?.value;
        this.characterId = document.getElementById('character-id')?.value;
        this.apiBaseUrl = window.apiBaseUrl || 'http://localhost:5000';
        this.currentNode = null;
        this.character = null;
        this.gameState = null;
        this.mapState = null;
        this.selectedHex = null;
        this.movementRemaining = 0;
        
        this.init();
    }

    async init() {
        try {
            await this.loadGameData();
            this.setupEventListeners();
            await this.loadMapState();
            await this.loadCurrentNode();
        } catch (error) {
            console.error('Failed to initialize adventure game:', error);
            this.showError('Failed to load game. Please try again.');
        }
    }

    setupEventListeners() {
        // Game control buttons
        document.getElementById('save-game')?.addEventListener('click', () => this.saveGame());
        document.getElementById('load-game')?.addEventListener('click', () => this.loadGame());
        document.getElementById('return-home')?.addEventListener('click', () => this.returnHome());
        document.getElementById('reset-movement')?.addEventListener('click', () => this.resetMovement());
        document.getElementById('move-to-hex')?.addEventListener('click', () => this.moveToSelectedHex());
        document.getElementById('explore-hex')?.addEventListener('click', () => this.exploreCurrentHex());
    }

    async loadGameData() {
        try {
            // Load game state first (which contains character ID)
            if (this.gameStateId) {
                const gameStateResponse = await fetch(`${this.apiBaseUrl}/api/game/${this.gameStateId}`);
                if (gameStateResponse.ok) {
                    this.gameState = await gameStateResponse.json();
                    this.updateGameStateDisplay();
                    
                    // Get character ID from game state if not provided
                    if (!this.characterId && this.gameState.characterId) {
                        this.characterId = this.gameState.characterId;
                    }
                }
            }

            // Load character data using the GameController endpoint
            if (this.characterId) {
                const characterResponse = await fetch(`${this.apiBaseUrl}/api/game/character/${this.characterId}`);
                if (characterResponse.ok) {
                    this.character = await characterResponse.json();
                    this.updateCharacterDisplay();
                } else {
                    console.warn('Character endpoint not found, character data may not be available');
                }
            }
        } catch (error) {
            console.error('Error loading game data:', error);
            throw error;
        }
    }

    updateCharacterDisplay() {
        if (!this.character) return;

        // Update character info
        document.getElementById('character-name').textContent = this.character.name || 'Unknown';
        document.getElementById('character-race').textContent = this.character.race || '--';
        document.getElementById('character-class').textContent = this.character.class || '--';
        document.getElementById('character-level').textContent = this.character.level || 1;

        // Update ability scores from Attributes dictionary
        const attributes = this.character.attributes || {};
        document.getElementById('str-value').textContent = attributes.Strength || attributes.strength || 10;
        document.getElementById('dex-value').textContent = attributes.Dexterity || attributes.dexterity || 10;
        document.getElementById('con-value').textContent = attributes.Constitution || attributes.constitution || 10;
        document.getElementById('int-value').textContent = attributes.Intelligence || attributes.intelligence || 10;
        document.getElementById('wis-value').textContent = attributes.Wisdom || attributes.wisdom || 10;
        document.getElementById('cha-value').textContent = attributes.Charisma || attributes.charisma || 10;

        // Update HP
        const currentHP = this.character.healthPoints || this.character.currentHitPoints || this.character.maxHealthPoints || 10;
        const maxHP = this.character.maxHealthPoints || this.character.maxHitPoints || 10;
        document.getElementById('hp-current').textContent = `${currentHP}/${maxHP}`;
        const xp = this.character.experiencePoints || 0;
        const nextLevel = this.character.experienceToNextLevel || 100;
        const xpElement = document.getElementById('xp-current');
        if (xpElement) {
            xpElement.textContent = `${xp}/${nextLevel}`;
        }

        // Update inventory
        this.updateInventoryDisplay();
    }

    updateInventoryDisplay() {
        const inventoryContainer = document.getElementById('inventory-items');
        if (!inventoryContainer) return;

        const inventory = this.character?.inventory || this.character?.equipment || [];
        if (inventory && inventory.length > 0) {
            inventoryContainer.innerHTML = '';
            inventory.forEach(item => {
                const itemElement = document.createElement('span');
                itemElement.className = 'inventory-item';
                itemElement.textContent = item.name || item;
                inventoryContainer.appendChild(itemElement);
            });
        } else {
            inventoryContainer.innerHTML = '<span class="inventory-item">No items</span>';
        }
    }

    updateGameStateDisplay() {
        if (!this.gameState) return;

        // Update location
        document.getElementById('current-location').textContent = this.gameState.currentLocation || 'Unknown';

        // Update quests
        this.updateQuestDisplay();
    }

    updateQuestDisplay() {
        const activeQuestsList = document.getElementById('active-quests');
        const completedQuestsList = document.getElementById('completed-quests');

        if (activeQuestsList) {
            if (this.gameState?.activeQuests && this.gameState.activeQuests.length > 0) {
                activeQuestsList.innerHTML = '';
                this.gameState.activeQuests.forEach(quest => {
                    const li = document.createElement('li');
                    li.className = 'list-group-item';
                    li.textContent = quest;
                    activeQuestsList.appendChild(li);
                });
            } else {
                activeQuestsList.innerHTML = '<li class="list-group-item">No active quests</li>';
            }
        }

        if (completedQuestsList) {
            if (this.gameState?.completedQuests && this.gameState.completedQuests.length > 0) {
                completedQuestsList.innerHTML = '';
                this.gameState.completedQuests.forEach(quest => {
                    const li = document.createElement('li');
                    li.className = 'list-group-item';
                    li.textContent = quest;
                    completedQuestsList.appendChild(li);
                });
            } else {
                completedQuestsList.innerHTML = '<li class="list-group-item">No completed quests</li>';
            }
        }
    }

    async loadMapState() {
        if (!this.characterId) return;

        try {
            const response = await fetch(`${this.apiBaseUrl}/api/map/state/${this.characterId}`);
            if (!response.ok) {
                throw new Error('Map state unavailable');
            }

            const previousLimit = this.mapState?.hexesPerTurn;
            this.mapState = await response.json();

            if (!previousLimit || this.movementRemaining <= 0) {
                this.movementRemaining = this.mapState.hexesPerTurn || 0;
            } else {
                this.movementRemaining = Math.min(this.movementRemaining, this.mapState.hexesPerTurn || this.movementRemaining);
            }

            const playerCell = this.mapState.cells.find(cell => cell.x === this.mapState.playerX && cell.y === this.mapState.playerY);
            this.selectedHex = playerCell ? { ...playerCell, distance: 0, inRange: false, isPlayer: true } : null;
            this.renderHexMap();
        } catch (error) {
            console.warn('Unable to load map state:', error);
            const map = document.getElementById('hex-map');
            if (map) {
                map.innerHTML = '<div class="alert alert-warning">Map is not available for this character yet.</div>';
            }
        }
    }

    renderHexMap() {
        const mapContainer = document.getElementById('hex-map');
        if (!mapContainer || !this.mapState) return;

        document.getElementById('map-name').textContent = this.mapState.mapName || 'Unknown Map';
        document.getElementById('movement-speed').textContent = `${this.mapState.movementFeet || 30} ft`;
        document.getElementById('movement-left').textContent = `${this.movementRemaining}/${this.mapState.hexesPerTurn}`;
        document.getElementById('hex-position').textContent = `${this.mapState.playerX}, ${this.mapState.playerY}`;
        document.getElementById('current-location').textContent = this.mapState.mapName || this.gameState?.currentLocation || 'Unknown';

        mapContainer.innerHTML = '';

        this.mapState.cells.forEach(cell => {
            const distance = this.getHexDistance(this.mapState.playerX, this.mapState.playerY, cell.x, cell.y);
            const inRange = cell.passable && distance > 0 && distance <= this.movementRemaining;
            const isPlayer = cell.x === this.mapState.playerX && cell.y === this.mapState.playerY;
            const isSelected = this.selectedHex && this.selectedHex.x === cell.x && this.selectedHex.y === cell.y;

            const button = document.createElement('button');
            button.type = 'button';
            button.className = [
                'hex-cell',
                `terrain-${(cell.terrainType || 'plains').toLowerCase()}`,
                cell.passable ? '' : 'blocked',
                inRange ? 'in-range' : '',
                isPlayer ? 'player' : '',
                isSelected ? 'selected' : '',
                cell.isEntryPoint ? 'entry' : ''
            ].filter(Boolean).join(' ');
            button.textContent = this.getHexLabel(cell, isPlayer);
            button.title = `${cell.name} (${cell.x}, ${cell.y})`;
            button.disabled = !cell.passable;
            button.addEventListener('click', () => this.selectHex(cell, distance, inRange, isPlayer));
            mapContainer.appendChild(button);
        });

        this.updateHexDetail();
        this.displayHexChoices();
    }

    getHexLabel(cell, isPlayer) {
        if (isPlayer) return 'You';
        if (cell.hasNpc) return 'NPC';
        if (cell.hasPointOfInterest) return '!';
        if (cell.hasStructure) return 'B';
        if (cell.isEntryPoint) return 'Entry';
        return cell.terrainType?.substring(0, 1) || '.';
    }

    selectHex(cell, distance, inRange, isPlayer) {
        this.selectedHex = {
            ...cell,
            distance,
            inRange,
            isPlayer
        };
        this.renderHexMap();
    }

    addChoiceButton(label, handler, disabled = false) {
        const choicesContainer = document.getElementById('choices-container');
        if (!choicesContainer) return;

        const button = document.createElement('button');
        button.className = 'choice-button';
        button.textContent = label;
        button.disabled = disabled;
        button.addEventListener('click', handler);
        choicesContainer.appendChild(button);
    }

    displayHexChoices() {
        const choicesContainer = document.getElementById('choices-container');
        if (!choicesContainer || !this.selectedHex) return;

        choicesContainer.innerHTML = '';

        const storyDescription = document.getElementById('story-description');
        if (storyDescription) {
            storyDescription.textContent = `${this.selectedHex.name}: ${this.selectedHex.description || 'Open ground.'}`;
        }

        if (!this.selectedHex.passable) {
            this.addChoiceButton('This hex is blocked', () => {}, true);
            return;
        }

        if (this.selectedHex.isPlayer) {
            this.addChoiceButton(`Explore ${this.selectedHex.name}`, () => this.exploreCurrentHex(false));

            if (this.selectedHex.pointOfInterestId && this.selectedHex.pointOfInterestActions?.length) {
                this.selectedHex.pointOfInterestActions.forEach(actionName => {
                    this.addChoiceButton(actionName, () => this.interactWithSelectedPOI(actionName));
                });
            }

            this.addChoiceButton('Reset movement for a new turn', () => this.resetMovement());
            return;
        }

        const moveLabel = this.selectedHex.inRange
            ? `Move to ${this.selectedHex.name} (${this.selectedHex.distance * 5} ft)`
            : `Move to ${this.selectedHex.name} (out of range)`;

        this.addChoiceButton(moveLabel, () => this.moveToSelectedHex(), !this.selectedHex.inRange);

        if (this.selectedHex.hasPointOfInterest) {
            this.addChoiceButton('Move here first to interact with this point', () => {}, true);
        }

        if (this.selectedHex.isEntryPoint) {
            this.addChoiceButton('Use as an entry point for the next area', () => {}, true);
        }
    }

    updateHexDetail() {
        const detail = document.getElementById('hex-detail');
        const moveButton = document.getElementById('move-to-hex');
        const poiActions = document.getElementById('poi-actions');
        if (!detail || !moveButton) return;

        if (poiActions) {
            poiActions.innerHTML = '';
        }

        if (!this.selectedHex) {
            detail.textContent = 'Select a highlighted hex to inspect it.';
            moveButton.disabled = true;
            this.displayHexChoices();
            return;
        }

        const tags = [];
        if (this.selectedHex.isEntryPoint) tags.push('Entry point');
        if (this.selectedHex.hasNpc) tags.push('NPC');
        if (this.selectedHex.hasPointOfInterest) tags.push('Point of interest');
        if (this.selectedHex.hasStructure) tags.push('Structure');
        if (!this.selectedHex.passable) tags.push('Blocked');

        detail.innerHTML = `
            <div><strong>${this.selectedHex.name}</strong></div>
            <div class="text-muted mb-2">${this.selectedHex.terrainType} at ${this.selectedHex.x}, ${this.selectedHex.y}</div>
            <p class="mb-2">${this.selectedHex.description || 'Open ground.'}</p>
            <div><strong>Distance:</strong> ${this.selectedHex.distance} hex${this.selectedHex.distance === 1 ? '' : 'es'}</div>
            <div><strong>Move cost:</strong> ${this.selectedHex.distance * 5} ft</div>
            ${tags.length ? `<div class="mt-2">${tags.map(tag => `<span class="badge bg-secondary me-1">${tag}</span>`).join('')}</div>` : ''}
        `;

        moveButton.disabled = this.selectedHex.isPlayer || !this.selectedHex.inRange || !this.selectedHex.passable;

        if (poiActions && this.selectedHex.pointOfInterestId && this.selectedHex.pointOfInterestActions?.length) {
            this.selectedHex.pointOfInterestActions.forEach(actionName => {
                const actionButton = document.createElement('button');
                actionButton.type = 'button';
                actionButton.className = 'btn btn-success';
                actionButton.textContent = actionName;
                actionButton.disabled = !this.selectedHex.isPlayer;
                actionButton.title = this.selectedHex.isPlayer ? '' : 'Move onto this hex before interacting.';
                actionButton.addEventListener('click', () => this.interactWithSelectedPOI(actionName));
                poiActions.appendChild(actionButton);
            });
        }

        this.displayHexChoices();
    }

    async moveToSelectedHex() {
        if (!this.selectedHex || !this.selectedHex.inRange || !this.characterId) return;

        try {
            const response = await fetch(`${this.apiBaseUrl}/api/map/move-to/${this.characterId}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ x: this.selectedHex.x, y: this.selectedHex.y })
            });

            const result = await response.json();
            if (!response.ok || !result.success) {
                throw new Error(result.message || 'Move failed');
            }

            this.movementRemaining = Math.max(0, this.movementRemaining - this.selectedHex.distance);
            await this.loadMapState();
            await this.exploreCurrentHex(false);
            this.displayHexChoices();
        } catch (error) {
            console.error('Move failed:', error);
            this.showError(error.message || 'Unable to move to that hex.');
        }
    }

    resetMovement() {
        if (!this.mapState) return;
        this.movementRemaining = this.mapState.hexesPerTurn || 0;
        this.renderHexMap();
        this.displayHexChoices();
    }

    async exploreCurrentHex(showAlert = true) {
        if (!this.characterId) return;

        try {
            const response = await fetch(`${this.apiBaseUrl}/api/map/explore/${this.characterId}`);
            const result = await response.json();

            if (!response.ok || !result.success) {
                throw new Error(result.message || 'Explore failed');
            }

            const storyDescription = document.getElementById('story-description');
            if (storyDescription) {
                storyDescription.textContent = result.cellDescription || result.message || `You examine ${result.cellName}.`;
            }

            this.displayHexChoices();

            if (showAlert) {
                this.showError(`Explored: ${result.cellName || 'current hex'}`);
            }
        } catch (error) {
            console.error('Explore failed:', error);
            this.showError(error.message || 'Unable to explore this hex.');
        }
    }

    async interactWithSelectedPOI(actionName) {
        if (!this.selectedHex?.pointOfInterestId || !this.characterId) return;

        try {
            const response = await fetch(`${this.apiBaseUrl}/api/map/interact/${this.characterId}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    pointOfInterestId: this.selectedHex.pointOfInterestId,
                    action: actionName
                })
            });

            const result = await response.json();
            if (!response.ok || !result.success) {
                throw new Error(result.message || 'Interaction failed');
            }

            const storyDescription = document.getElementById('story-description');
            if (storyDescription) {
                const rewards = [];
                if (result.experienceGained) rewards.push(`+${result.experienceGained} XP`);
                if (result.leveledUp) rewards.push(`Level ${result.newLevel}`);
                if (result.questsStarted?.length) rewards.push(`Quest started: ${result.questsStarted.join(', ')}`);
                if (result.questsCompleted?.length) rewards.push(`Quest completed: ${result.questsCompleted.join(', ')}`);
                storyDescription.textContent = [result.message, rewards.join(' | ')].filter(Boolean).join(' ');
            }

            await this.loadGameData();
            await this.loadMapState();
            this.displayHexChoices();
        } catch (error) {
            console.error('Interaction failed:', error);
            this.showError(error.message || 'Unable to interact with this point.');
        }
    }

    getHexDistance(ax, ay, bx, by) {
        const a = this.offsetToCube(ax, ay);
        const b = this.offsetToCube(bx, by);
        return Math.max(Math.abs(a.x - b.x), Math.abs(a.y - b.y), Math.abs(a.z - b.z));
    }

    offsetToCube(col, row) {
        const x = col - Math.floor((row - (row & 1)) / 2);
        const z = row;
        const y = -x - z;
        return { x, y, z };
    }

    async loadCurrentNode() {
        if (!this.gameStateId) {
            this.showError('No game state found');
            return;
        }

        try {
            this.showLoading(true);
            
            const response = await fetch(`${this.apiBaseUrl}/api/game/${this.gameStateId}/node`);
            if (response.ok) {
                this.currentNode = await response.json();
                this.displayCurrentNode();
            } else {
                throw new Error('Failed to load current adventure node');
            }
        } catch (error) {
            console.error('Error loading current node:', error);
            this.showError('Failed to load adventure content. Please try again.');
        } finally {
            this.showLoading(false);
        }
    }

    displayCurrentNode() {
        if (!this.currentNode) return;

        // Update story description
        const storyDescription = document.getElementById('story-description');
        if (storyDescription) {
            storyDescription.textContent = this.currentNode.description || 'The adventure continues...';
        }

        // Display choices
        this.displayChoices();
        this.displayHexChoices();

        // Display NPC interactions
        this.displayNPCInteractions();

        // Show story content
        document.getElementById('story-content').style.display = 'block';
    }

    displayChoices() {
        const choicesContainer = document.getElementById('choices-container');
        if (!choicesContainer) return;

        choicesContainer.innerHTML = '';

        if (this.currentNode.choices && this.currentNode.choices.length > 0) {
            this.currentNode.choices.forEach((choice, index) => {
                const button = document.createElement('button');
                button.className = 'choice-button';
                button.textContent = choice.text;
                button.addEventListener('click', () => this.makeChoice(index));
                choicesContainer.appendChild(button);
            });
        } else {
            const noChoicesMessage = document.createElement('p');
            noChoicesMessage.className = 'text-muted';
            noChoicesMessage.textContent = 'No choices available. The adventure continues...';
            choicesContainer.appendChild(noChoicesMessage);
        }
    }

    displayNPCInteractions() {
        const npcInteractions = document.getElementById('npc-interactions');
        const npcButtons = document.getElementById('npc-buttons');
        
        if (!npcInteractions || !npcButtons) return;

        if (this.currentNode.npcInteractions && this.currentNode.npcInteractions.length > 0) {
            npcButtons.innerHTML = '';
            
            this.currentNode.npcInteractions.forEach(npc => {
                const button = document.createElement('button');
                button.className = 'npc-button';
                button.textContent = `${npc.npcName} (${npc.interactionType})`;
                button.title = npc.interactionDescription;
                button.addEventListener('click', () => this.interactWithNPC(npc));
                npcButtons.appendChild(button);
            });
            
            npcInteractions.style.display = 'block';
        } else {
            npcInteractions.style.display = 'none';
        }
    }

    async makeChoice(choiceIndex) {
        if (!this.gameStateId) {
            this.showError('No game state found');
            return;
        }

        try {
            // Disable all choice buttons
            const choiceButtons = document.querySelectorAll('.choice-button');
            choiceButtons.forEach(button => button.disabled = true);

            this.showLoading(true);

            const response = await fetch(`${this.apiBaseUrl}/api/game/${this.gameStateId}/choice/${choiceIndex}`, {
                method: 'POST'
            });

            if (response.ok) {
                this.currentNode = await response.json();
                this.displayCurrentNode();
                
                // Reload game state to get updated information
                await this.loadGameData();
            } else {
                throw new Error('Failed to process choice');
            }
        } catch (error) {
            console.error('Error making choice:', error);
            this.showError('Failed to process your choice. Please try again.');
            
            // Re-enable choice buttons
            const choiceButtons = document.querySelectorAll('.choice-button');
            choiceButtons.forEach(button => button.disabled = false);
        } finally {
            this.showLoading(false);
        }
    }

    async interactWithNPC(npc) {
        // For now, just show an alert with NPC info
        // This could be expanded to handle different interaction types
        alert(`Interacting with ${npc.npcName}: ${npc.interactionDescription}`);
        
        // TODO: Implement actual NPC interaction logic
        // This would involve calling an API endpoint for NPC interactions
    }

    showLoading(show) {
        const loadingSpinner = document.getElementById('loading-spinner');
        const storyContent = document.getElementById('story-content');
        
        if (loadingSpinner && storyContent) {
            if (show) {
                loadingSpinner.style.display = 'block';
                storyContent.style.display = 'none';
            } else {
                loadingSpinner.style.display = 'none';
                storyContent.style.display = 'block';
            }
        }
    }

    showError(message) {
        // Create or update error message
        let errorDiv = document.getElementById('error-message');
        if (!errorDiv) {
            errorDiv = document.createElement('div');
            errorDiv.id = 'error-message';
            errorDiv.className = 'alert alert-danger';
            errorDiv.style.margin = '20px 0';
            
            const container = document.querySelector('.adventure-container');
            if (container) {
                container.insertBefore(errorDiv, container.firstChild);
            }
        }
        
        errorDiv.innerHTML = `
            <strong>Error:</strong> ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        `;
        errorDiv.style.display = 'block';
    }

    async saveGame() {
        try {
            // TODO: Implement save game functionality
            // This would call an API endpoint to save the current game state
            alert('Save game functionality not yet implemented');
        } catch (error) {
            console.error('Error saving game:', error);
            this.showError('Failed to save game');
        }
    }

    async loadGame() {
        try {
            // TODO: Implement load game functionality
            // This would show a dialog to select a saved game
            alert('Load game functionality not yet implemented');
        } catch (error) {
            console.error('Error loading game:', error);
            this.showError('Failed to load game');
        }
    }

    returnHome() {
        if (confirm('Are you sure you want to return to the main menu? Any unsaved progress will be lost.')) {
            window.location.href = '/';
        }
    }
}

// Initialize the adventure game when the page loads
document.addEventListener('DOMContentLoaded', () => {
    new AdventureGame();
});
