# Ogre Battle Army Tool

https://bacteriamage.wordpress.com/2021/07/24/ogre-battle-army-tool/

## Introduction

The _Ogre Battle Army Tool_ is a utility for [_Ogre Battle: The March of the Black Queen_](https://ogrebattlesaga.fandom.com/wiki/Ogre_Battle:_The_March_of_the_Black_Queen) for the Super Nintendo. The program can be used to both export and import the complete set of characters and units from and to any of the save slots in the game's SRAM. This has many possible uses including archiving unused characters, manipulating character statistics, and transferring characters, units, or even entire armies between saved games.

## Windows Interface

This tool uses a simple text-only (or "console") user interface but still supports basic use from Windows Explorer for users who are unfamiliar with or would simply prefer not to use the console interface. In this mode data can only be imported to or exported from the saved game in slot 1 and is accomplished by dragging and dropping files directly to the program's icon in Explorer. A press-any-key prompt will appear before the window closes to allow any messages to be read.

To export units and characters, drag only the SRAM data file and drop it onto the tool program's icon. The content from the saved game in slot 1 will be exported to a file in the same folder as the SRAM file and with the same name but with a CSV extension. To import units and characters, select both a SRAM file and a CSV file then drag and drop both of them together onto the tool program's icon. If the CSV file contains valid characters and units then they will be written to the saved game in slot 1.

## Console Interface

The tool also accepts command-line parameters when run from the Command Prompt, Windows PowerShell, or other console. The same import and export capabilities are provided but this mode gives more control over the specific location and file name used for the SRAM and CSV files. It also allows any of the game's three save slots to be used. When the program is run without any parameters it will display help detailing the specific values that are expected.

## Working with SRAM

A SRAM file is always required to import or to export since it contains the saved game that the data is exported from or imported into. This file contains a copy of the battery-backed RAM from the Ogre Battle cartridge where the saved games are stored and is always exactly 8 KB (8,192 bytes) in size. Emulators will load and save games from this same file and it can also be read directly from a cartridge using an appropriate cartridge reader. The SRAM file must have either a ".srm" or ".sav" extension or it will not be recognized by the tool.

## Working with Exports

Exports produced by this tool are CSV (comma separated values) files that contain all the saved game's characters and their unit assignments. This type of file consists of cells of data in columns and rows and can be read and written to by any spreadsheet application (e.g. Microsoft Excel, OpenOffice, etc.). Each column in the CSV file represents a particular data point for a character, such as the character's name or strength attribute.

The first row of the file contains the column headers that identify the information stored in that column. The column headers are required for import so they cannot be removed or renamed but it is acceptable to rearrange the order of the columns or remove empty columns entirely. Each row represents a single character and contains all the data needed by the game for that character in the individual cells.

## Working with Imports

An import always replaces all the existing characters and units in the saved gamed with whatever is in the import CSV file. Anything in the saved game but not in the import is simply erased. For this reason the import must always be a complete army and include all the characters (including the Lord character) and not only those that are to be added or updated. To update only certain characters, first export the entire army, then make the desired modifications leaving the other rows unchanged, and finally import the entire file containing the updated and non-updated characters back into the saved game.

Before writing the imported data to the saved game the tool will validate it to make sure that it meets the game's rules and will function correctly. If any issues are discovered then the import will be aborted (no changes are saved) and one or more error messages will be displayed. The message will describe the issue with the data as well as the column name and row number where the issue was found. Use this information to update the CSV file to address the problem and then try the import again.

## Columns

**Number** - The index number Ogre Battle uses to keep track of the character. Later Ogre Battle games show this number in the game but MOTB keeps it hidden and so it's mostly a curiosity. It's safe to ignore these numbers when working with export or imports but they can be used or reassigned as desired. The cell can also be left blank for some or all of the characters and the import will assign numbers to blanks and fix any conflicts automatically.

**Name** - The name of the character as it appears in the game. For the Lord character this is the player name as entered at the start of the game and can be changed by entering a new name in the cell as long as it fits the length and letter restrictions. For all other characters this must match one of the pre-defined values in the game's bank of about 1,700 hard-coded names.

**Identity** - For special characters (e.g. Lans) this column identifies the specific individual by name. The name column controls the display name only so it's possible to change the character's name without changing the identity. For all other characters this cell is blank.

**Class** - The current class (e.g. Knight) of the character.
 
**EquippedItem** - The item equipped on the character. When an item is equipped in-game it is completely removed from inventory so removing equipment from the import effectively removes it from the game. Likewise, importing a new item adds it to the game for free.

**Unit** - The number (1 to 25) of the unit the character is assigned to. Leave this cell and the other unit cells blank for reserve characters not assigned to any unit.

**UnitRow** - The row of the unit where the character is positioned - Front or Back.

**UnitPosition** - The position of the character within the assigned row. From the top: RightFlank, RightCenter, Center, LeftCenter, LeftFlank. The game requires at least one empty space between any two characters in the same row.

**UnitLeader** - Whether the character is the leader of the unit: Yes or No. Can also be blank for non-leader units.

**The Level**, **HitPoints**, **Strength**, **Agility**, **Intelligence**, **Charisma**, **Alignment**, **Luck**, **Exp**, and **Salary** columns correspond directly with the in-game statistic.

## Tables

For the Name, Identity, Class, and EquippedItem columns, the game actually uses somewhat arbitrary numbers to represent each possible value. It would be very difficult to work with the data in this way since it would require knowing what each number means. Instead, table files are used to convert to and from human-friendly names for each number that appears in the CSV file. If a character has an unexpected value that doesn't exist in the table then the cell will express it as a hexadecimal code (e.g 0x1A) in the export. Arbitrary values can also be entered into any of these columns for import by entering a hexadecimal code.

The table files can be opened for viewing in any text editor (e.g. Notepad) to look up the names of values that can be used in the respective column. This is especially useful for the Name column when naming a new character or renaming an existing one. Alternatively, the names in the tables can be altered to customize what each value is called. Custom tables also potentially allow for compatibility with ROM hacks that alter any of this information in the game itself. The table files must be in the same folder as the army tool program file or they will not be found.

**Names.txt** - All the possible names for characters. This file is organized into four sections for male characters, female characters, large characters, and special characters. Each section has been alphabetized but in some cases the same name appears more than once or in multiple sections and so has an extra notation (e.g. "Carlos(2)") in order to make each instance of the same name unique.

**Identities.txt** - The personal identity codes for all of the special characters in the game.

**Classes.txt** - The names of all the character classes (e.g. Knight) in the game. This file also contains extra columns with additional metadata for each class. The information is needed to correctly recompute certain data structures in the SRAM file when importing characters and units and can generally be ignored by the user.

**Items.txt** - The names of weapons, armor, and other items that characters can be equipped with.

