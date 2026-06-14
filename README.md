# Console RPG: Mage v Dragon

A turn-based command-line RPG built in C#. This project focuses on safe user input, state management, and game loops, demonstrating basic c# software engineering patterns.

## Concepts & Architecture

The game goes with a structured **Input Validation & Execution Pattern** to ensure the game never crashes from bad user input. The core architecture is broken into four distinct phases per turn:

1. **Inner Loop:** A localized `while(true)` loop that traps the user during the decision phase. It safely parses string inputs into integers using `int.TryParse` and verifies array boundaries to prevent `IndexOutOfRangeException` errors.
2. **Resource Guards:** Validates game state (e.g., checking if the Mage has enough Mana) before allowing an action to proceed.
3. **The Router:** Handles intentional cancellations (typing `0` to go back) and failed resource checks, cleanly routing the user back to the main menu without invoking any action.
4. **State Mutation (Action Phase):** Once an action is fully validated, the game state is permanently altered (e.g., Mana is consumed, Health is reduced), and the enemy responds.

## Current Features

- **Spellcasting System:** Choose from a list of attack spells.
- **Utility System:** Cast non-damaging spells (like Healing or Buffs). **_Invisible_** currently has empty functionalitiy, aswell as **_Attack_** from the main manu.
- **Crash-Proof Menus:** Users cannot crash the game by typing letters or out-of-bounds numbers.
- **Mana Management:** Spells correctly check for and consume required mana resources.
- **Dragon Attack:** The dragon attacks functions with a randomiser.

## Roadmap (What's Missing)

While the core engine is robust, the game loop is missing a few critical pieces to be a fully playable game:

### 1. Case "3": The Basic Attack

Currently, if the Wizard runs out of Mana, they have no way to deal damage. We need to implement `case "3"` to allow a basic, resource-free melee attack (e.g., hitting the dragon with a staff).

### 2. Win / Loss States

The main `do...while` loop ends when either the Dragon or the Wizard reaches 0 Health. However, there is no code _after_ the loop to announce the winner. We need a "Game Over" screen that checks who survived and prints a victory or defeat message.

### 3. Code Refactoring (DRY Principle)

`case "1"` and `case "2"` currently use almost identical validation loops. To make the code easier to maintain, I should extract the `while(true)` loop into a separate helper method (e.g., `GetValidSpellChoice()`) so i don't repeat the code.
