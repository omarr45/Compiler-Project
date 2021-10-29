| Abbreviation | Original      |
| ------------ | ------------- |
| `exp`        | Expression    |
| `id`         | Identifier    |
| `asgmnt`     | Assignment    |
| `dt`         | Data Type     |
| `cond`       | Condition     |
| `bop`        | Bool Operator |

11. := `exp`
12. INT | FLOAT | STRING
13. `dt id asgmnt`? (, `id asgmnt`?)\* ;
14. WRITE `exp` | endl ;
15. READ `id` ;
16. RETURN `exp` ;
17. < | > | = | <>
18. `id cond term`
19. && | ||
20. `cond` (`bop cond`)\*
