# Apos.History
A C# library that makes it easy to handle undo and redo.

[![Discord](https://img.shields.io/discord/257949867551358987.svg)](https://discord.rashtal.com/)

## Documentation

* [Getting started](https://apostolique.github.io/Apos.History/getting-started/)

## Build

[![NuGet](https://img.shields.io/nuget/v/Apos.History.svg)](https://www.nuget.org/packages/Apos.History/) [![NuGet](https://img.shields.io/nuget/dt/Apos.History.svg)](https://www.nuget.org/packages/Apos.History/)

## Features

* Undo
* Redo
* History from multiple data structures.
* Remove earliest history if needed.

## Usage

```csharp
var historyHandler = new HistoryHandler();

int fishCount = 0;
int appleCount = 0;

SaveFishCount(fishCount, 3);
SaveFishCount(fishCount, 4);
SaveFishCount(fishCount, 5);

SaveAppleCount(appleCount, 7);
SaveAppleCount(appleCount, 9);
SaveAppleCount(appleCount, 4);
SaveAppleCount(appleCount, 5);

// Group multiple histories in one batch.
historyHandler.AutoCommit = false;
SaveFishCount(fishCount, 10);
SaveAppleCount(appleCount, 20);
// Call Commit manually.
historyHandler.Commit();
historyHandler.AutoCommit = true;

historyHandler.Undo();
historyHandler.Undo();

historyHandler.Redo();

SaveFishCount(int oldCount, int newCount) {
    historyHandler.Add(() => {
        fishCount = oldCount;
    }, () => {
        fishCount = newCount;
    });
}
SaveAppleCount(int oldCount, int newCount) {
    historyHandler.Add(() => {
        appleCount = oldCount;
    }, () => {
        appleCount = newCount;
    });
}
```
