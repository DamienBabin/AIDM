// Adventure Game JavaScript
class AdventureGame {
    constructor() {
        this.gameStateId = document.getElementById('game-state-id')?.value;
        this.characterId = document.getElementById('character-id')?.value;
        this.apiBaseUrl = window.apiBaseUrl || 'https://localhost:7001';
        this.currentNode = null;
        this.character = null;
        this.gameState = null;
        
        this.init();
    }

    async init() {
        try {
            await this.loadGameData();
            this.setupEventListeners();
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
    }

    async loadGameData() {
        try {
            // Load character data
            if (this.characterId) {
                const characterResponse = await fetch(`${this.apiBaseUrl}/api/game/character/${this.characterId}`);
                if (characterResponse.ok) {
                    this.character = await characterResponse.json();
                    this.updateCharacterDisplay();
                }
            }

            // Load game state
            if (this.gameStateId) {
                const gameStateResponse = await fetch(`${this.apiBaseUrl}/api/game/${this.gameStateId}`);
                if (gameStateResponse.ok) {
                    this.gameState = await gameStateResponse.json();
                    this.updateGameStateDisplay();
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

        // Update ability scores
        document.getElementById('str-value').textContent = this.character.strength || 10;
        document.getElementById('dex-value').textContent = this.character.dexterity || 10;
        document.getElementById('con-value').textContent = this.character.constitution || 10;
        document.getElementById('int-value').textContent = this.character.intelligence || 10;
        document.getElementById('wis-value').textContent = this.character.wisdom || 10;
        document.getElementById('cha-value').textContent = this.character.charisma || 10;

        // Update HP
        document.getElementById('hp-current').textContent = `${this.character.currentHitPoints || this.character.maxHitPoints || 10}/${this.character.maxHitPoints || 10}`;

        // Update inventory
        this.updateInventoryDisplay();
    }

    updateInventoryDisplay() {
        const inventoryContainer = document.getElementById('inventory-items');
        if (!inventoryContainer) return;

        if (this.character?.equipment && this.character.equipment.length > 0) {
            inventoryContainer.innerHTML = '';
            this.character.equipment.forEach(item => {
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
