# DayzTreeRandomizer
Small Winforms app that can randomize scale and rotation of your DayZ trees from a Terrain Builder object export file
## DayZ Tree Randomizer
*Usage
-First, backup your project! - While I have tested this app and had no issues, it's always good to backup your project, also I don't want to be blamed for anything.
-Export the trees you want to edit through Terrain Builders 'export - objects' command. If you have a group of trees selected, choose the 'Selection' option, or 'Active Layer' if you have a dedicated layer for trees. Choose the types of trees you want exported, or click 'select all'. I recommend testing a small selection first to see how the app works.
-Open the app, select your file you just exported. Choose whether you want to scale, rotate, or both, and enter a scale range.
-Hit the export button - the new export file will always go in same folder as your original file, with the extension "_export" added to the file name
-Drag the new exported file into your project in Terrain Builder - I recommend creating a new layer before doing this, and import the changed trees into this layer. This way you can verify the correct about of trees have been imported in.
-Once you have verified all you newly scaled/rotated trees have been imported successfully, you can remove your existing trees/layer.

*Things to note - I've only added some simple checks to verify you give the app a valid terrain builder txt file, so make sure you do! 
                - The app will rotate / scale any type of object that is in the file - ie if you included some buildings, the app isn't checking for tree object file names
