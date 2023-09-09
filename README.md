## What's this
A simple console app that checks for changes in specified files. The changes are detected between each run of the app.
## Installation
Put [the compiled binary file](https://github.com/Mufanza/has-it-changed/tree/master/Binaries) into the folder in which you want to track changes. It will just work.

See the *Configuration* section bellow if you want to tweak it somehow.
## How does it work,
The first time you run the app, it will do a quick scan through all files in the folder it resides in (as well as all of its subfolders). The results of this scan are saved to a file. Each subsequent time you run the app, the scan is repeated and its results are compared with the results of the previous run.
The app ends with exit code 1 if changes are detected to any of the files.
The app ends with exit code 0 if no changes are detected (or if this is the first time running the app, or if the results from the previous scan are not found)

The app might exit with -1 if anything goes unexpectedly wrong.

## Motivation
You might need to put this into your deployment pipelines to do magical stuff; it's not meant to be used as a version control.

In other words: either you know why you might need to use it, or you don't need to use it.

## Configuration
Putting the compiled .exe into the root folder and running it will just work.

You may run it with the following flags:
**-silent (-s):** make it not output anything to the console
**-diff (-d):** prints what changes were found where (if not ran with -s)

For more configuration, you may also create a file called 'HasItChanged_Config.json' in the root folder. You may fill it with the following optional parameters:
**FileExtensions:** An array of file extensions; specifies what file types should be scanned. All file types will be checked for changes if this is omitted. Otherwise, the checker will only be interested in files specified in here.
**Root:** You can use this to set the root folder to be somewhere else. If omitted, the folder from which the app is ran will be set as root.
**PathToPastDataFile:** Similarly, you can use this to change where the scan results will be saved.

Example:
```
{
  "FileExtensions": [ ".txt", ".py" ],
  "Root": "C:\\SomeOtherFolder\\MyProject",
  "PathToPastDataFile": "C:\\SomewhereElse\\HasItChanged_scan_results.json"
}
```

## Notes
The app will create a new file in the root folder called 'HasItChanged_FileStructure.json' (it uses it to store the scan results from the previous run). You might want to add this file into .gitignore

The app uses filesizes and SHA265 alghorithm to check for changes. Therefore, there's an astronomically low chance for size+hash collision (in other words, there's a very small chance that a change done to a file will go undetected).
