# Scaling operators

Mod version: 0.1.0-rc1

> Note: This guide is for alpha versions of the Skills++ mod.
> All code examples and described methods may change in future releases.
> **Breaking changes will occur as the mod evolves.**

The scaling operators provided in the Skills++ codebase are designed to make it easier to implement values that scale based on the character's level.
Using them may also help maintain compatability with the internal behaviour of the Skills++ mod.
**It is highly recommended to use these methods when possible.**

## `AdditiveScaling`

The additive scaling operator *adds a constant amount* every level.
This is suited well for integer values, such as a skill's stock count.

```c#
    int value = (int)AdditiveScaling(3, 2, level); // increase by 2 per level
```

Example:

| Level | Value |
|-------|--------|
| 1 | 3 |
| 2 | 5 |
| 3 | 7 |
| 4 | 9 |

Additive scaling can also be used with non-integer amounts which can be useful a skill's buff is granted every few levels.

```c#
    int value = (int)AdditiveScaling(0, 0.5f, level); // increase by 0.5 per level rounding down
```

| Level | Value |
|-------|--------|
| 1 | 0 |
| 2 | 0 |
| 3 | 1 |
| 4 | 1 |

## `MultScaling`

The additive scaling operator *multiples by a constant amount* every level.
The repeated multiplication of the same constant results is compounding.

```c#
    float value = MultScaling(100f, 0.5f, level); // increase by 50% per level
```

| Level | Value |
|-------|-------|
| 1 | 100 |
| 2 | 150 |
| 3 | 225 |
| 4 | 337.5 |

Multipliers between -1 and 0 will cause the value to converge on 0.

```c#
    float value = MultScaling(100f, -0.1f, level); // reduce by 10% every level
```

| Level | Value |
|-------|-------|
| 1 | 100 |
| 2 | 90 |
| 3 | 81 |
| 4 | 72.9 |

Multipliers less than -1 are not allowed and you will recieve an assertion error if you attempt this.
