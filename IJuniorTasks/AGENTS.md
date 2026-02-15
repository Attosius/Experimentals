# AGENTS.md

This file contains guidelines for agentic coding assistants working on this .NET 6.0 C# project.

## Build Commands

- `dotnet build` - Build the project
- `dotnet run` - Execute the program

**Note**: This codebase contains multiple `.cs` files with duplicate `Program` classes and `Main` methods (e.g., `Main31`, `Main41`, `Main51`, etc.). When running the project, execution is determined by the active `Main` entry point in `Program.cs:10`.

## Testing

No test framework is configured in this project. This is a collection of junior-level programming exercises rather than a production application.

## Code Style Guidelines

### Naming Conventions

- **Classes**: PascalCase (e.g., `Player`, `Zoo`, `Enclosure`)
- **Methods**: PascalCase (e.g., `DrawPlayer`, `VisitEnclosure`)
- **Properties**: PascalCase (e.g., `PositionX`, `Name`)
- **Public fields**: PascalCase (rare - prefer properties)
- **Private instance fields**: `_camelCase` with underscore prefix (e.g., `_enclosures`, `_enclosureFactory`)
- **Private static fields**: `s_camelCase` with s_ prefix
- **Constants**: PascalCase (e.g., `CommandRubToUsd`)
- **Local variables**: `camelCase` using `var` (e.g., `var number = 10;`)
- **Enums**: PascalCase (e.g., `Commands`, `Gender`)

### Class Member Ordering

Organize class members in this order with blank lines between sections:

1. Constants
2. Fields (sorted by access: public → protected → private)
3. Constructors
4. Properties (sorted by access: public → protected → private)
5. Methods (sorted by access: public → protected → private)

Example:
```csharp
public class Example
{
    // Constants
    private const int MaxCount = 100;

    // Fields
    private readonly List<Item> _items;
    private string _name;

    // Constructors
    public Example(string name)
    {
        _name = name;
        _items = new List<Item>();
    }

    // Properties
    public string Name { get; }
    public int Count => _items.Count;

    // Public Methods
    public void Add(Item item)
    {
        _items.Add(item);
    }

    // Private Methods
    private void Validate()
    {
        // implementation
    }
}
```

### Imports and Namespaces

- Use `using System;` and other required namespaces at the top of files
- Each type file uses a namespace (typically `IJuniorTasks` or variants like `IJuniorTasks61`)
- `ImplicitUsings` is disabled in csproj - explicit `using` statements required

### Formatting

- Use PascalCase for all public members (properties, methods, classes)
- Use string interpolation: `$"{variable}"` instead of concatenation
- Use `var` for local variables when type is obvious
- Properties should use `{ get; }` for read-only
- Use `readonly` modifier for fields that are only set in constructor

### Error Handling

- Use `TryParse` for user input validation: `int.TryParse(input, out result)`
- Validate command inputs: `Enum.TryParse<Commands>(command, out var commandEnum)`
- Check array bounds before accessing: `index >= 0 && index < list.Count`
- Return early or use continue on invalid input

### Additional Guidelines

- **No class name in members**: Don't repeat class name in fields/properties/methods (e.g., use `_name` not `_playerName` inside Player class)
- **Encapsulation**: Use private fields with public properties. Avoid public fields.
- **Reference types**: Don't pass mutable reference types outside the class if they should only be modified internally
- **Console output**: Use `Console.Clear()`, `Console.Write()`, `Console.WriteLine()` for CLI applications
- **Static utility methods**: Create separate utility classes (e.g., `UserUtils`) for reusable static methods

### csproj Configuration

- Target Framework: `net6.0`
- Output Type: `Exe` (console application)
- ImplicitUsings: `disable` (explicit using statements required)
- Nullable: `disable` (reference types are non-nullable by default)
