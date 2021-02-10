# Getting Started

## Install

Install using the following dotnet command:
```
dotnet add package Apos.History
```

## Setup

Import the library with:
```csharp
using Apos.History;
```

## Types

| Type | Description |
| ---- | ----------- |
| History | Base class that is useful to build your own history wrappers. |
| HistoryHandler | Used to group multiple History together and have only one combined undo or redo stack. |
| HistorySet | Represents a single state in the history. Useful for grouping many actions together. |

## Usage

Note: This library builds history using lambdas and [closures](https://en.wikipedia.org/wiki/Closure_(computer_programming)). It's a good practice not to capture reference types so that the object isn't held on to forever in the history.

Let's say you want to preserve the history of an `int`. You can write the following class:

```csharp
public class HistoryInt : History {
    public HistoryInt() { }
    public HistoryInt(HistoryHandler? historyHandler) : base(historyHandler) { }

    public int Value {
        get => _value;
        set {
            Save(_value, value);
        }
    }

    private void Save(int oldValue, int newValue) {
        _pendingUndo.Add(() => {
            _value = oldValue;
        });
        _pendingRedo.Add(() => {
            _value = newValue;
        });

        TryCommit();
    }

    private int _value;
}
```

This class manages an int called `_value` and exposes the `Value` property. If the history is managed locally, in other word no `HistoryHandler` was given to the constructor, then you can call `Undo()` and `Redo()` on it.

In order to build the history, you can add some `Action` to the `_pendingUndo` and `_pendingRedo` lists. These will get applied to the history by `Commit()`. `Commit()` is called by `TryCommit()` if `AutoCommit` is set to `true`.
