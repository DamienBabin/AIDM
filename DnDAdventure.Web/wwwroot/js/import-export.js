// Import/Export functionality for D&D Adventure
class ImportExportManager {
    constructor() {
        this.apiBaseUrl = window.apiBaseUrl || 'http://localhost:5000';
        this.init();
    }

    init() {
        this.setupEventListeners();
    }

    setupEventListeners() {
        // Import button
        const importBtn = document.getElementById('btn-import');
        if (importBtn) {
            importBtn.addEventListener('click', () => {
                this.importData();
            });
        }

        // Export button
        const exportBtn = document.getElementById('btn-export');
        if (exportBtn) {
            exportBtn.addEventListener('click', () => {
                this.exportData();
            });
        }

        // World selection
        const worldSelect = document.getElementById('world-select');
        if (worldSelect) {
            worldSelect.addEventListener('change', (e) => {
                this.onWorldChange(e.target.value);
            });
        }
    }

    async importData() {
        const fileInput = document.getElementById('import-file');
        const file = fileInput.files[0];

        if (!file) {
            this.showMessage('Please select a JSON file to import.', 'warning');
            return;
        }

        if (!file.name.toLowerCase().endsWith('.json')) {
            this.showMessage('Please select a valid JSON file.', 'error');
            return;
        }

        try {
            this.showMessage('Importing world data...', 'info');
            
            const formData = new FormData();
            formData.append('file', file);

            const response = await fetch(`${this.apiBaseUrl}/api/save/import-file`, {
                method: 'POST',
                body: formData
            });

            if (response.ok) {
                const result = await response.json();
                this.showMessage(`Successfully imported world: ${result.worldName || 'Unknown'}`, 'success');
                
                // Clear the file input
                fileInput.value = '';
                
                // Refresh world list
                await this.loadWorlds();
                
                // Update world info if available
                if (result.worldName) {
                    this.updateWorldInfo(result);
                }
            } else {
                const errorText = await response.text();
                this.showMessage(`Import failed: ${errorText}`, 'error');
            }
        } catch (error) {
            console.error('Import error:', error);
            this.showMessage(`Import failed: ${error.message}`, 'error');
        }
    }

    async exportData() {
        try {
            this.showMessage('Exporting current world data...', 'info');
            
            const response = await fetch(`${this.apiBaseUrl}/api/save/quicksave`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                const result = await response.json();
                this.showMessage(`World exported successfully: ${result.fileName}`, 'success');
                
                // Optionally trigger download
                if (result.downloadUrl) {
                    window.open(result.downloadUrl, '_blank');
                }
            } else {
                const errorText = await response.text();
                this.showMessage(`Export failed: ${errorText}`, 'error');
            }
        } catch (error) {
            console.error('Export error:', error);
            this.showMessage(`Export failed: ${error.message}`, 'error');
        }
    }

    async loadWorlds() {
        try {
            const response = await fetch(`${this.apiBaseUrl}/api/save`);
            if (response.ok) {
                const saves = await response.json();
                this.populateWorldSelect(saves);
            }
        } catch (error) {
            console.error('Error loading worlds:', error);
        }
    }

    populateWorldSelect(saves) {
        const worldSelect = document.getElementById('world-select');
        if (!worldSelect) return;

        // Clear existing options except the first two
        while (worldSelect.children.length > 2) {
            worldSelect.removeChild(worldSelect.lastChild);
        }

        // Add saved worlds
        saves.forEach(save => {
            const option = document.createElement('option');
            option.value = save.filePath;
            option.textContent = save.worldName || save.fileName;
            option.dataset.worldData = JSON.stringify(save);
            worldSelect.appendChild(option);
        });
    }

    async onWorldChange(value) {
        const worldInfo = document.getElementById('world-info');
        if (!worldInfo) return;

        if (value === 'new') {
            // Show new world form
            const newWorldForm = document.getElementById('new-world-form');
            if (newWorldForm) {
                newWorldForm.style.display = 'block';
            }
            
            worldInfo.innerHTML = '<p class="text-muted">Fill in the details to create a new world</p>';
        } else if (value) {
            // Hide new world form
            const newWorldForm = document.getElementById('new-world-form');
            if (newWorldForm) {
                newWorldForm.style.display = 'none';
            }

            // Load selected world
            try {
                const response = await fetch(`${this.apiBaseUrl}/api/save/${encodeURIComponent(value)}`);
                if (response.ok) {
                    const worldData = await response.json();
                    this.updateWorldInfo(worldData);
                    this.showMessage(`Loaded world: ${worldData.name}`, 'success');
                }
            } catch (error) {
                console.error('Error loading world:', error);
                this.showMessage('Failed to load world details', 'error');
            }
        } else {
            // Hide new world form
            const newWorldForm = document.getElementById('new-world-form');
            if (newWorldForm) {
                newWorldForm.style.display = 'none';
            }
            
            worldInfo.innerHTML = '<p class="text-muted">Select a world to view details</p>';
        }
    }

    updateWorldInfo(worldData) {
        const worldInfo = document.getElementById('world-info');
        if (!worldInfo) return;

        const characterCount = worldData.characters ? Object.keys(worldData.characters).length : 0;
        const npcCount = worldData.npcs ? Object.keys(worldData.npcs).length : 0;
        const locationCount = worldData.locations ? Object.keys(worldData.locations).length : 0;
        const questCount = worldData.quests ? Object.keys(worldData.quests).length : 0;

        worldInfo.innerHTML = `
            <h6>${worldData.name || 'Unknown World'}</h6>
            <p class="text-muted">${worldData.description || 'No description available'}</p>
            <hr>
            <div class="row text-center">
                <div class="col-6">
                    <div class="border rounded p-2 mb-2">
                        <strong>${characterCount}</strong><br>
                        <small class="text-muted">Characters</small>
                    </div>
                </div>
                <div class="col-6">
                    <div class="border rounded p-2 mb-2">
                        <strong>${npcCount}</strong><br>
                        <small class="text-muted">NPCs</small>
                    </div>
                </div>
                <div class="col-6">
                    <div class="border rounded p-2 mb-2">
                        <strong>${locationCount}</strong><br>
                        <small class="text-muted">Locations</small>
                    </div>
                </div>
                <div class="col-6">
                    <div class="border rounded p-2 mb-2">
                        <strong>${questCount}</strong><br>
                        <small class="text-muted">Quests</small>
                    </div>
                </div>
            </div>
            ${worldData.createdAt ? `<small class="text-muted">Created: ${new Date(worldData.createdAt).toLocaleDateString()}</small>` : ''}
        `;
    }

    showMessage(message, type = 'info') {
        // Create or update message container
        let messageContainer = document.getElementById('message-container');
        if (!messageContainer) {
            messageContainer = document.createElement('div');
            messageContainer.id = 'message-container';
            messageContainer.style.position = 'fixed';
            messageContainer.style.top = '20px';
            messageContainer.style.right = '20px';
            messageContainer.style.zIndex = '9999';
            document.body.appendChild(messageContainer);
        }

        // Create message element
        const messageElement = document.createElement('div');
        messageElement.className = `alert alert-${this.getBootstrapClass(type)} alert-dismissible fade show`;
        messageElement.style.minWidth = '300px';
        messageElement.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        messageContainer.appendChild(messageElement);

        // Auto-remove after 5 seconds
        setTimeout(() => {
            if (messageElement.parentNode) {
                messageElement.remove();
            }
        }, 5000);
    }

    getBootstrapClass(type) {
        switch (type) {
            case 'success': return 'success';
            case 'error': return 'danger';
            case 'warning': return 'warning';
            case 'info': 
            default: return 'info';
        }
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    window.importExportManager = new ImportExportManager();
});
