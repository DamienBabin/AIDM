// Character Creation JavaScript - Updated for Flattened Races
class CharacterCreator {
    constructor() {
        this.apiBaseUrl = window.apiBaseUrl || 'http://localhost:5000';
        this.selectedRaceData = null; // Store the flattened race data
        this.selectedClass = null;
        this.selectedSubclass = null;
        this.availableCantrips = [];
        this.selectedCantrips = [];
        this.maxCantrips = 0;
        this.abilityScores = {
            Strength: 10,
            Dexterity: 10,
            Constitution: 10,
            Intelligence: 10,
            Wisdom: 10,
            Charisma: 10
        };
        this.racialBonuses = {};
        this.pointBuyPoints = 27;
        this.pointBuyCosts = {
            8: 0, 9: 1, 10: 2, 11: 3, 12: 4, 13: 5, 14: 7, 15: 9
        };
        
        this.init();
    }

    async init() {
        await this.loadWorlds();
        await this.loadRaces();
        await this.loadClasses();
        this.setupEventListeners();
        this.updateCharacterPreview();
    }

    async loadWorlds() {
        try {
            const response = await fetch(`${this.apiBaseUrl}/api/world`);
            const worlds = await response.json();
            
            const worldSelect = document.getElementById('world-select');
            if (worldSelect) {
                // Keep the existing options and add loaded worlds
                worlds.forEach(world => {
                    const option = document.createElement('option');
                    option.value = world.id;
                    option.textContent = world.name;
                    worldSelect.appendChild(option);
                });
            }
        } catch (error) {
            console.error('Error loading worlds:', error);
        }
    }

    setupEventListeners() {
        // Race selection
        document.getElementById('race').addEventListener('change', (e) => {
            this.onRaceChange(e.target.value);
        });

        // Class selection
        document.getElementById('class').addEventListener('change', (e) => {
            this.onClassChange(e.target.value);
        });

        // Subclass selection
        document.getElementById('subclass').addEventListener('change', (e) => {
            this.onSubclassChange(e.target.value);
        });

        // Ability score method
        document.getElementById('ability-method').addEventListener('change', (e) => {
            this.onAbilityMethodChange(e.target.value);
        });

        // Standard array assignments
        document.querySelectorAll('.ability-assignment').forEach(select => {
            select.addEventListener('change', (e) => {
                this.onStandardArrayChange();
            });
        });

        // Custom stats
        document.querySelectorAll('.custom-stat').forEach(input => {
            input.addEventListener('input', (e) => {
                this.onCustomStatChange();
            });
        });

        // Character name and background
        document.getElementById('name').addEventListener('input', () => {
            this.updateCharacterPreview();
        });

        document.getElementById('background').addEventListener('change', () => {
            this.updateCharacterPreview();
        });

        // Create character button
        document.getElementById('create-character').addEventListener('click', () => {
            this.createCharacter();
        });

        // Reset form button
        document.getElementById('reset-form').addEventListener('click', () => {
            this.resetForm();
        });
    }

    async loadRaces() {
        try {
            const response = await fetch(`${this.apiBaseUrl}/api/classesraces/races/flattened`);
            const races = await response.json();
            
            const raceSelect = document.getElementById('race');
            raceSelect.innerHTML = '<option value="" selected disabled>Select Race</option>';
            
            races.forEach(race => {
                const option = document.createElement('option');
                option.value = race.name;
                option.textContent = race.displayName;
                raceSelect.appendChild(option);
            });
        } catch (error) {
            console.error('Error loading races:', error);
        }
    }

    async loadClasses() {
        try {
            const response = await fetch(`${this.apiBaseUrl}/api/classesraces/classes`);
            const classes = await response.json();
            
            const classSelect = document.getElementById('class');
            classSelect.innerHTML = '<option value="" selected disabled>Select Class</option>';
            
            classes.forEach(dndClass => {
                const option = document.createElement('option');
                option.value = dndClass.name;
                option.textContent = dndClass.name;
                classSelect.appendChild(option);
            });
        } catch (error) {
            console.error('Error loading classes:', error);
        }
    }

    async onRaceChange(raceName) {
        if (!raceName) return;

        try {
            // Get the flattened race data again to find the selected race
            const response = await fetch(`${this.apiBaseUrl}/api/classesraces/races/flattened`);
            const races = await response.json();
            
            // Find the selected race from the flattened list
            this.selectedRaceData = races.find(r => r.name === raceName);
            if (!this.selectedRaceData) {
                console.error('Selected race not found in flattened data');
                return;
            }
            
            // Hide the subrace dropdown since we're using flattened data
            document.getElementById('subrace').style.display = 'none';
            
            // Show race info
            this.displayRaceInfo();
            
            // Update racial bonuses
            this.updateRacialBonuses();
            
            // Update character preview
            this.updateCharacterPreview();
            
        } catch (error) {
            console.error('Error loading race details:', error);
        }
    }

    displayRaceInfo() {
        if (!this.selectedRaceData) return;

        const panel = document.getElementById('race-info-panel');
        const description = document.getElementById('race-description');
        const abilityScores = document.getElementById('race-ability-scores');
        const traits = document.getElementById('race-traits');

        panel.style.display = 'block';
        description.textContent = this.selectedRaceData.description;

        // Ability score increases
        abilityScores.innerHTML = '';
        Object.entries(this.selectedRaceData.abilityScoreIncrease).forEach(([ability, bonus]) => {
            const li = document.createElement('li');
            li.className = 'list-group-item d-flex justify-content-between';
            li.innerHTML = `<span>${ability}</span><span>+${bonus}</span>`;
            abilityScores.appendChild(li);
        });

        // Racial traits
        traits.innerHTML = '';
        this.selectedRaceData.traits.forEach(trait => {
            const li = document.createElement('li');
            li.innerHTML = `<strong>${trait.name}:</strong> ${trait.description}`;
            traits.appendChild(li);
        });

        // Display subrace info if this is a subrace
        if (this.selectedRaceData.isSubrace && this.selectedRaceData.pros && this.selectedRaceData.pros.length > 0) {
            this.displaySubraceInfo();
        }
    }

    displaySubraceInfo() {
        if (!this.selectedRaceData || !this.selectedRaceData.isSubrace) return;

        // Create or update subrace info panel
        let subracePanel = document.getElementById('subrace-info-panel');
        if (!subracePanel) {
            subracePanel = document.createElement('div');
            subracePanel.id = 'subrace-info-panel';
            subracePanel.className = 'mt-3 p-3 bg-light rounded';
            document.getElementById('race-info-panel').appendChild(subracePanel);
        }

        subracePanel.innerHTML = `
            <h6>${this.selectedRaceData.subraceName} Details</h6>
            ${this.selectedRaceData.pros && this.selectedRaceData.pros.length > 0 ? `
                <div class="row mt-3">
                    <div class="col-md-6">
                        <h6 class="text-success">✓ Pros</h6>
                        <ul class="list-unstyled">
                            ${this.selectedRaceData.pros.map(pro => `<li class="text-success">• ${pro}</li>`).join('')}
                        </ul>
                    </div>
                    <div class="col-md-6">
                        <h6 class="text-danger">✗ Cons</h6>
                        <ul class="list-unstyled">
                            ${this.selectedRaceData.cons && this.selectedRaceData.cons.length > 0 ? 
                                this.selectedRaceData.cons.map(con => `<li class="text-danger">• ${con}</li>`).join('') : 
                                '<li class="text-muted">• No significant drawbacks</li>'}
                        </ul>
                    </div>
                </div>
                ${this.selectedRaceData.bestFor && this.selectedRaceData.bestFor.length > 0 ? `
                    <div class="mt-3">
                        <h6 class="text-primary">🎯 Best For</h6>
                        <ul class="list-unstyled">
                            ${this.selectedRaceData.bestFor.map(use => `<li class="text-primary">• ${use}</li>`).join('')}
                        </ul>
                    </div>
                ` : ''}
                ${this.selectedRaceData.playStyle ? `
                    <div class="mt-3">
                        <h6 class="text-info">🎭 Play Style</h6>
                        <p class="text-info">${this.selectedRaceData.playStyle}</p>
                    </div>
                ` : ''}
            ` : ''}
        `;
    }

    async onClassChange(className) {
        if (!className) return;

        try {
            const response = await fetch(`${this.apiBaseUrl}/api/classesraces/class/${encodeURIComponent(className)}`);
            this.selectedClass = await response.json();
            
            // Load subclasses
            await this.loadSubclasses(className);
            
            // Show class info
            this.displayClassInfo();
            
            // Load cantrips if spellcasting class
            if (this.selectedClass.spellcasting) {
                await this.loadCantrips(className);
                this.showCantripSelection();
            } else {
                this.hideCantripSelection();
            }
            
            // Update character preview
            this.updateCharacterPreview();
            
        } catch (error) {
            console.error('Error loading class details:', error);
        }
    }

    async loadSubclasses(className) {
        const subclassSelect = document.getElementById('subclass');
        
        if (this.selectedClass.subclasses && this.selectedClass.subclasses.length > 0) {
            subclassSelect.style.display = 'block';
            subclassSelect.innerHTML = '<option value="" selected disabled>Select Subclass</option>';
            
            this.selectedClass.subclasses.forEach(subclass => {
                const option = document.createElement('option');
                option.value = subclass.name;
                option.textContent = subclass.name;
                subclassSelect.appendChild(option);
            });
        } else {
            subclassSelect.style.display = 'none';
            this.selectedSubclass = null;
        }
    }

    async onSubclassChange(subclassName) {
        if (!subclassName || !this.selectedClass) return;

        this.selectedSubclass = this.selectedClass.subclasses.find(s => s.name === subclassName);
        this.displaySubclassInfo();
        this.updateCharacterPreview();
    }

    displayClassInfo() {
        if (!this.selectedClass) return;

        const panel = document.getElementById('class-info-panel');
        const description = document.getElementById('class-description');
        const hitDie = document.getElementById('class-hit-die');
        const primaryAbilities = document.getElementById('class-primary-abilities');
        const savingThrows = document.getElementById('class-saving-throws');
        const armor = document.getElementById('class-armor');
        const weapons = document.getElementById('class-weapons');
        const features = document.getElementById('class-features');

        panel.style.display = 'block';
        description.textContent = this.selectedClass.description;
        hitDie.textContent = `d${this.selectedClass.hitDie}`;
        primaryAbilities.textContent = this.selectedClass.primaryAbilities.join(', ');
        savingThrows.textContent = this.selectedClass.savingThrowProficiencies.join(', ');
        armor.textContent = this.selectedClass.armorProficiencies.join(', ') || 'None';
        weapons.textContent = this.selectedClass.weaponProficiencies.join(', ');

        // Class features
        features.innerHTML = '';
        this.selectedClass.features.forEach(feature => {
            const div = document.createElement('div');
            div.className = 'mb-2';
            div.innerHTML = `<strong>${feature.name} (Level ${feature.levelUnlocked}):</strong> ${feature.description}`;
            features.appendChild(div);
        });
    }

    displaySubclassInfo() {
        if (!this.selectedSubclass) return;

        // Create or update subclass info panel
        let subclassPanel = document.getElementById('subclass-info-panel');
        if (!subclassPanel) {
            subclassPanel = document.createElement('div');
            subclassPanel.id = 'subclass-info-panel';
            subclassPanel.className = 'mt-3 p-3 bg-light rounded';
            document.getElementById('class-info-panel').appendChild(subclassPanel);
        }

        subclassPanel.innerHTML = `
            <h6>${this.selectedSubclass.name} Details</h6>
            <p><strong>Description:</strong> ${this.selectedSubclass.description}</p>
            ${this.selectedSubclass.features && this.selectedSubclass.features.length > 0 ? `
                <div class="mt-2">
                    <strong>Subclass Features:</strong>
                    ${this.selectedSubclass.features.map(feature => `
                        <div class="mb-2">
                            <strong>${feature.name} (Level ${feature.levelUnlocked}):</strong> ${feature.description}
                        </div>
                    `).join('')}
                </div>
            ` : ''}
        `;
    }

    async loadCantrips(className) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/api/spells/cantrips/${encodeURIComponent(className)}`);
            this.availableCantrips = await response.json();
            this.maxCantrips = this.selectedClass.spellcasting.cantripsKnown;
            this.selectedCantrips = [];
            
            this.displayAvailableCantrips();
            this.updateCantripSelection();
        } catch (error) {
            console.error('Error loading cantrips:', error);
        }
    }

    showCantripSelection() {
        document.getElementById('cantrip-selection').style.display = 'block';
        document.getElementById('cantrip-info').innerHTML = `
            <p class="text-muted">Your ${this.selectedClass.name} can learn ${this.maxCantrips} cantrips at 1st level.</p>
        `;
    }

    hideCantripSelection() {
        document.getElementById('cantrip-selection').style.display = 'none';
        this.selectedCantrips = [];
        this.availableCantrips = [];
    }

    displayAvailableCantrips() {
        const container = document.getElementById('available-cantrips');
        container.innerHTML = '';

        this.availableCantrips.forEach(cantrip => {
            const div = document.createElement('div');
            div.className = 'cantrip-item';
            div.dataset.cantripName = cantrip.name;
            div.innerHTML = `
                <strong>${cantrip.name}</strong><br>
                <small class="text-muted">${cantrip.school} • ${cantrip.castingTime}</small>
            `;
            
            div.addEventListener('click', () => this.toggleCantrip(cantrip));
            container.appendChild(div);
        });
    }

    toggleCantrip(cantrip) {
        const index = this.selectedCantrips.findIndex(c => c.name === cantrip.name);
        
        if (index >= 0) {
            // Remove cantrip
            this.selectedCantrips.splice(index, 1);
        } else if (this.selectedCantrips.length < this.maxCantrips) {
            // Add cantrip
            this.selectedCantrips.push(cantrip);
        }
        
        this.updateCantripSelection();
        this.updateCharacterPreview();
    }

    updateCantripSelection() {
        // Update available cantrips display
        document.querySelectorAll('.cantrip-item').forEach(item => {
            const cantripName = item.dataset.cantripName;
            const isSelected = this.selectedCantrips.some(c => c.name === cantripName);
            const canSelect = this.selectedCantrips.length < this.maxCantrips;
            
            item.classList.toggle('selected', isSelected);
            item.classList.toggle('disabled', !isSelected && !canSelect);
        });

        // Update selected cantrips display
        const selectedContainer = document.getElementById('selected-cantrips');
        const countSpan = document.getElementById('selected-count');
        const maxSpan = document.getElementById('max-cantrips');
        
        countSpan.textContent = this.selectedCantrips.length;
        maxSpan.textContent = this.maxCantrips;

        if (this.selectedCantrips.length === 0) {
            selectedContainer.innerHTML = '<p class="text-muted">No cantrips selected</p>';
        } else {
            selectedContainer.innerHTML = '';
            this.selectedCantrips.forEach(cantrip => {
                const div = document.createElement('div');
                div.className = 'cantrip-item selected';
                div.innerHTML = `
                    <strong>${cantrip.name}</strong><br>
                    <small class="text-muted">${cantrip.school} • ${cantrip.castingTime}</small>
                `;
                div.addEventListener('click', () => this.showCantripDetails(cantrip));
                selectedContainer.appendChild(div);
            });
        }
    }

    showCantripDetails(cantrip) {
        const detailsContainer = document.getElementById('cantrip-details');
        detailsContainer.innerHTML = `
            <h6>${cantrip.name}</h6>
            <p><strong>School:</strong> ${cantrip.school}</p>
            <p><strong>Casting Time:</strong> ${cantrip.castingTime}</p>
            <p><strong>Range:</strong> ${cantrip.range}</p>
            <p><strong>Components:</strong> ${cantrip.components}</p>
            <p><strong>Duration:</strong> ${cantrip.duration}</p>
            <p><strong>Description:</strong> ${cantrip.description}</p>
        `;
    }

    onAbilityMethodChange(method) {
        const panel = document.getElementById('ability-score-panel');
        const methodName = document.getElementById('selected-method-name');
        
        // Hide all method panels
        document.getElementById('standard-array').style.display = 'none';
        document.getElementById('point-buy').style.display = 'none';
        document.getElementById('roll-stats').style.display = 'none';
        document.getElementById('custom-stats').style.display = 'none';

        if (method) {
            panel.style.display = 'block';
            
            switch (method) {
                case 'standard':
                    methodName.textContent = 'Standard Array';
                    document.getElementById('standard-array').style.display = 'block';
                    this.resetStandardArray();
                    break;
                case 'pointbuy':
                    methodName.textContent = 'Point Buy';
                    document.getElementById('point-buy').style.display = 'block';
                    this.resetPointBuy();
                    break;
                case 'roll':
                    methodName.textContent = 'Roll 4d6 (drop lowest)';
                    document.getElementById('roll-stats').style.display = 'block';
                    this.resetRolledStats();
                    break;
                case 'custom':
                    methodName.textContent = 'Custom Values';
                    document.getElementById('custom-stats').style.display = 'block';
                    this.resetCustomStats();
                    break;
            }
        } else {
            panel.style.display = 'none';
        }
    }

    resetStandardArray() {
        document.querySelectorAll('.ability-assignment').forEach(select => {
            select.value = '';
        });
        this.abilityScores = {
            Strength: 10, Dexterity: 10, Constitution: 10,
            Intelligence: 10, Wisdom: 10, Charisma: 10
        };
        this.updateCharacterPreview();
    }

    onStandardArrayChange() {
        const assignments = {};
        const usedValues = [];
        
        document.querySelectorAll('.ability-assignment').forEach(select => {
            const ability = select.dataset.ability;
            const value = parseInt(select.value) || 10;
            assignments[ability] = value;
            if (select.value) usedValues.push(select.value);
        });

        // Update available options
        document.querySelectorAll('.ability-assignment').forEach(select => {
            const currentValue = select.value;
            Array.from(select.options).forEach(option => {
                if (option.value && option.value !== currentValue) {
                    option.disabled = usedValues.includes(option.value);
                }
            });
        });

        this.abilityScores = assignments;
        this.updateCharacterPreview();
    }

    resetPointBuy() {
        this.pointBuyPoints = 27;
        Object.keys(this.abilityScores).forEach(ability => {
            this.abilityScores[ability] = 8;
            const input = document.getElementById(`pb-${ability.toLowerCase()}`);
            if (input) input.value = 8;
            const cost = document.getElementById(`pb-${ability.toLowerCase()}-cost`);
            if (cost) cost.textContent = '0';
        });
        document.getElementById('points-remaining').textContent = '27';
        this.updateCharacterPreview();
    }

    adjustPointBuy(ability, change) {
        const currentValue = this.abilityScores[ability];
        const newValue = currentValue + change;
        
        if (newValue < 8 || newValue > 15) return;
        
        const currentCost = this.getPointBuyCost(currentValue);
        const newCost = this.getPointBuyCost(newValue);
        const costDifference = newCost - currentCost;
        
        if (this.pointBuyPoints - costDifference < 0) return;
        
        this.abilityScores[ability] = newValue;
        this.pointBuyPoints -= costDifference;
        
        const input = document.getElementById(`pb-${ability.toLowerCase()}`);
        if (input) input.value = newValue;
        
        const cost = document.getElementById(`pb-${ability.toLowerCase()}-cost`);
        if (cost) cost.textContent = newCost;
        
        document.getElementById('points-remaining').textContent = this.pointBuyPoints;
        this.updateCharacterPreview();
    }

    getPointBuyCost(value) {
        return this.pointBuyCosts[value] || 0;
    }

    resetRolledStats() {
        Object.keys(this.abilityScores).forEach(ability => {
            this.abilityScores[ability] = 10;
            const input = document.getElementById(`roll-${ability.toLowerCase()}`);
            if (input) input.value = '';
            const detail = document.getElementById(`roll-${ability.toLowerCase()}-detail`);
            if (detail) detail.textContent = '';
        });
        this.updateCharacterPreview();
    }

    rollStat(ability) {
        const rolls = [];
        for (let i = 0; i < 4; i++) {
            rolls.push(Math.floor(Math.random() * 6) + 1);
        }
        rolls.sort((a, b) => b - a);
        const total = rolls.slice(0, 3).reduce((sum, roll) => sum + roll, 0);
        
        this.abilityScores[ability] = total;
        
        const input = document.getElementById(`roll-${ability.toLowerCase()}`);
        if (input) input.value = total;
        
        const detail = document.getElementById(`roll-${ability.toLowerCase()}-detail`);
        if (detail) detail.textContent = `Rolled: ${rolls.join(', ')} (dropped ${rolls[3]})`;
        
        this.updateCharacterPreview();
    }

    rollAllStats() {
        Object.keys(this.abilityScores).forEach(ability => {
            this.rollStat(ability);
        });
    }

    resetCustomStats() {
        Object.keys(this.abilityScores).forEach(ability => {
            this.abilityScores[ability] = 10;
            const input = document.getElementById(`custom-${ability.toLowerCase()}`);
            if (input) input.value = 10;
        });
        this.updateCharacterPreview();
    }

    onCustomStatChange() {
        Object.keys(this.abilityScores).forEach(ability => {
            const input = document.getElementById(`custom-${ability.toLowerCase()}`);
            if (input) {
                this.abilityScores[ability] = parseInt(input.value) || 10;
            }
        });
        this.updateCharacterPreview();
    }

    updateRacialBonuses() {
        this.racialBonuses = {};
        
        if (this.selectedRaceData) {
            Object.entries(this.selectedRaceData.abilityScoreIncrease).forEach(([ability, bonus]) => {
                if (ability !== "Any Two Different") {
                    this.racialBonuses[ability] = (this.racialBonuses[ability] || 0) + bonus;
                }
            });
        }
    }

    updateCharacterPreview() {
        // Update final ability scores
        Object.keys(this.abilityScores).forEach(ability => {
            const baseScore = this.abilityScores[ability];
            const racialBonus = this.racialBonuses[ability] || 0;
            const finalScore = baseScore + racialBonus;
            const modifier = Math.floor((finalScore - 10) / 2);
            
            const element = document.getElementById(`${ability.substring(0, 3).toLowerCase()}-value`);
            if (element) {
                element.textContent = `${finalScore} (${modifier >= 0 ? '+' : ''}${modifier})`;
            }
        });

        // Update character summary
        const nameElement = document.getElementById('preview-name');
        const raceElement = document.getElementById('preview-race');
        const classElement = document.getElementById('preview-class');
        const backgroundElement = document.getElementById('preview-background');

        if (nameElement) {
            nameElement.textContent = document.getElementById('name').value || '-';
        }
        if (raceElement) {
            raceElement.textContent = this.selectedRaceData ? this.selectedRaceData.displayName : '-';
        }
        if (classElement) {
            let classText = this.selectedClass ? this.selectedClass.name : '-';
            if (this.selectedSubclass) {
                classText += ` (${this.selectedSubclass.name})`;
            }
            classElement.textContent = classText;
        }
        if (backgroundElement) {
            backgroundElement.textContent = document.getElementById('background').value || '-';
        }

        // Update equipment tab
        this.updateEquipmentTab();
        
        // Update spells tab
        this.updateSpellsTab();
    }

    updateEquipmentTab() {
        const startingEquipment = document.getElementById('starting-equipment');
        const armorProfs = document.getElementById('armor-proficiencies');
        const weaponProfs = document.getElementById('weapon-proficiencies');
        const savingThrowProfs = document.getElementById('saving-throw-proficiencies');

        if (this.selectedClass) {
            // Starting equipment
            startingEquipment.innerHTML = '';
            this.selectedClass.startingEquipment.forEach(item => {
                const li = document.createElement('li');
                li.className = 'list-group-item';
                li.textContent = item;
                startingEquipment.appendChild(li);
            });

            // Proficiencies
            armorProfs.textContent = this.selectedClass.armorProficiencies.join(', ') || 'None';
            weaponProfs.textContent = this.selectedClass.weaponProficiencies.join(', ') || 'None';
            savingThrowProfs.textContent = this.selectedClass.savingThrowProficiencies.join(', ') || 'None';
        } else {
            startingEquipment.innerHTML = '<li class="list-group-item text-muted">Select a class to see starting equipment</li>';
            armorProfs.textContent = 'Select a class to see armor proficiencies';
            weaponProfs.textContent = 'Select a class to see weapon proficiencies';
            savingThrowProfs.textContent = 'Select a class to see saving throw proficiencies';
        }
    }

    updateSpellsTab() {
        const knownCantrips = document.getElementById('known-cantrips');
        const spellcastingInfo = document.getElementById('spellcasting-info');

        if (this.selectedCantrips.length > 0) {
            knownCantrips.innerHTML = '';
            this.selectedCantrips.forEach(cantrip => {
                const li = document.createElement('li');
                li.className = 'list-group-item';
                li.innerHTML = `<strong>${cantrip.name}</strong><br><small class="text-muted">${cantrip.school} cantrip</small>`;
                knownCantrips.appendChild(li);
            });
        } else {
            knownCantrips.innerHTML = '<li class="list-group-item text-muted">No cantrips selected</li>';
        }

        if (this.selectedClass && this.selectedClass.spellcasting) {
            const spellcasting = this.selectedClass.spellcasting;
            spellcastingInfo.innerHTML = `
                <p><strong>Spellcasting Ability:</strong> ${spellcasting.spellcastingAbility}</p>
                <p><strong>Cantrips Known:</strong> ${spellcasting.cantripsKnown}</p>
                <p><strong>1st Level Spell Slots:</strong> ${spellcasting.spellSlots}</p>
                <p><strong>Ritual Casting:</strong> ${spellcasting.ritualCasting ? 'Yes' : 'No'}</p>
                <p><strong>Spellcasting Focus:</strong> ${spellcasting.spellcastingFocus}</p>
            `;
        } else {
            spellcastingInfo.innerHTML = '<p class="text-muted">Select a spellcasting class to see spellcasting information</p>';
        }
    }

    async createCharacter() {
        // Validate required fields
        const name = document.getElementById('name').value.trim();
        if (!name) {
            alert('Please enter a character name.');
            return;
        }

        if (!this.selectedRaceData) {
            alert('Please select a race.');
            return;
        }

        if (!this.selectedClass) {
            alert('Please select a class.');
            return;
        }

        // Check if spellcasting class has required cantrips
        if (this.selectedClass.spellcasting && this.selectedCantrips.length < this.maxCantrips) {
            const remaining = this.maxCantrips - this.selectedCantrips.length;
            if (!confirm(`You have ${remaining} cantrip(s) remaining to select. Create character anyway?`)) {
                return;
            }
        }

        // Calculate final ability scores with racial bonuses
        const finalAbilityScores = {};
        Object.keys(this.abilityScores).forEach(ability => {
            finalAbilityScores[ability] = this.abilityScores[ability] + (this.racialBonuses[ability] || 0);
        });

        // Collect all racial traits
        const allRacialTraits = this.selectedRaceData.traits.map(t => t.name);
        
        // Collect all languages
        const allLanguages = [...this.selectedRaceData.languages];
        
        // Calculate speed
        let speed = this.selectedRaceData.speed;

        // Create character object
        const character = {
            name: name,
            background: document.getElementById('background').value,
            race: this.selectedRaceData.raceName,
            subrace: this.selectedRaceData.isSubrace ? this.selectedRaceData.subraceName : null,
            class: this.selectedClass.name,
            subclass: this.selectedSubclass ? this.selectedSubclass.name : null,
            baseAttributes: { ...this.abilityScores },
            racialBonuses: { ...this.racialBonuses },
            attributes: { ...finalAbilityScores },
            racialTraits: allRacialTraits,
            cantrips: this.selectedCantrips.map(c => c.name),
            languages: allLanguages,
            speed: speed,
            level: 1,
            healthPoints: this.selectedClass.hitDie + Math.floor((finalAbilityScores.Constitution - 10) / 2),
            maxHealthPoints: this.selectedClass.hitDie + Math.floor((finalAbilityScores.Constitution - 10) / 2),
            armorClass: 10 + Math.floor((finalAbilityScores.Dexterity - 10) / 2)
        };

        console.log('Character created:', character);
        try {
            // Disable the create button to prevent double-clicking
            const createButton = document.getElementById('create-character');
            createButton.disabled = true;
            createButton.textContent = 'Creating Character...';

            // Check if world is selected
            const selectedWorld = document.getElementById('world-select').value;
            if (!selectedWorld) {
                alert('Please select a world first.');
                createButton.disabled = false;
                createButton.textContent = '🎭 Create Character';
                return;
            }

            // Save the character
            const characterResponse = await fetch(`${this.apiBaseUrl}/api/game/character`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(character)
            });

            if (!characterResponse.ok) {
                throw new Error('Failed to create character');
            }

            const savedCharacter = await characterResponse.json();
            console.log('Character saved:', savedCharacter);

            // Create a new game state
            const gameStateData = {
                characterId: savedCharacter.id,
                worldId: selectedWorld === 'new' ? null : selectedWorld,
                worldName: selectedWorld === 'new' ? document.getElementById('world-name').value : null,
                worldDescription: selectedWorld === 'new' ? document.getElementById('world-description').value : null
            };

            const gameStateResponse = await fetch(`${this.apiBaseUrl}/api/game/start`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(gameStateData)
            });

            if (!gameStateResponse.ok) {
                throw new Error('Failed to start game');
            }

            const gameState = await gameStateResponse.json();
            console.log('Game started:', gameState);

            // Redirect to adventure page
            window.location.href = `/Adventure?gameStateId=${gameState.id}&characterId=${savedCharacter.id}`;

        } catch (error) {
            console.error('Error creating character or starting game:', error);
            alert('Failed to create character and start game. Please try again.');
            
            // Re-enable the create button
            const createButton = document.getElementById('create-character');
            createButton.disabled = false;
            createButton.textContent = '🎭 Create Character';
        }
    }

    resetForm() {
        if (!confirm('Are you sure you want to reset the entire form? This will clear all your selections.')) {
            return;
        }

        // Reset all form fields
        document.getElementById('name').value = '';
        document.getElementById('background').value = '';
        document.getElementById('race').value = '';
        document.getElementById('subrace').style.display = 'none';
        document.getElementById('class').value = '';
        document.getElementById('subclass').style.display = 'none';
        document.getElementById('ability-method').value = '';

        // Reset internal state
        this.selectedRaceData = null;
        this.selectedClass = null;
        this.selectedSubclass = null;
        this.availableCantrips = [];
        this.selectedCantrips = [];
        this.maxCantrips = 0;
        this.abilityScores = {
            Strength: 10, Dexterity: 10, Constitution: 10,
            Intelligence: 10, Wisdom: 10, Charisma: 10
        };
        this.racialBonuses = {};

        // Hide panels
        document.getElementById('race-info-panel').style.display = 'none';
        document.getElementById('class-info-panel').style.display = 'none';
        document.getElementById('cantrip-selection').style.display = 'none';
        document.getElementById('ability-score-panel').style.display = 'none';

        // Update character preview
        this.updateCharacterPreview();
    }
}

// Global functions for onclick handlers
function adjustPointBuy(ability, change) {
    if (window.characterCreator) {
        window.characterCreator.adjustPointBuy(ability, change);
    }
}

function rollStat(ability) {
    if (window.characterCreator) {
        window.characterCreator.rollStat(ability);
    }
}

function rollAllStats() {
    if (window.characterCreator) {
        window.characterCreator.rollAllStats();
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    window.characterCreator = new CharacterCreator();
});
