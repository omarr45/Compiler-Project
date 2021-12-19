# Tiny Language CFG Rules

- Non-Terminals
    1. Function_call
    2. Term
    3. Arithmatic_Op ??
    4. Equation
    5. Expression
    6. Assignment_Statement
    7. Datatype ??
    8. Declaration_Statement
    9. Write_Statement
    10. Read_Statement
    11. Return_Statement
    12. Condition_Operator ??
    13. Condition
    14. Boolean_Operator ??
    15. Condition_Statement
    16. If_Statement
    17. Else_If_Statement
    18. Else_Statement
    19. Repeat_Statement
    20. Parameter
    21. Function_Declaration
    22. Function_Body
    23. Function_Statement
    24. Main_Function
    25. Program
    
1. Program **→** Program' Main_Function
2. **Program'** → Function_Statement Program' | e
3. Main_Function → Datatype main ( ) Function_Body
4. Function_Statment → Function_Declaration Function_Body
5. Function_Body → { **Statements** Return_Statment }
6. **Statements →** **Statement** **Statements'***
7. **Statements' →** **Statement Statements'** | e*
8. **Statement → Full_Assignment_Statement** | Declaration_Statement | Write_Statement | Read_Statement | Return_Statement | Condition_Statement | If_Statement | Else_If_Statement | Else_Statement | Repeat_Statement ??
9. Function_Declaration → Datatype Function_Name ( **Parameters** )
10. **Parameters →** Parameter **Parameters'** | e*
11. **Parameters' →** , Parameter **Parameters'** | e
12. Parameter → Datatype Identifier
13. Function_Name → Identifier
14. Repeat_Statement → repeat **Statements** until Condition_Statement
15. Else_Statement → else **Statements** end ??
16. Else_If_Statement → else If_Statement
17. If_Statement → if Condition_Statement  then **Statements If'**
18. **If' →** Else_If_Statement | Else_Statement |  end 
19. Condition_Statement → Condition **Conditions** 
20. **Conditions** → Boolean_Op Condition **Conditions** | e
21. **Full_Assignment_Statement →** Assignment_Statement ;
22. Return_Statement → return Expression ;
23. Read_Statement → read Identifier ;
24. Write_Statement → write **Write'**
25. **Write'** → Expression ; | Endline ;
26. Declaration_Statement → Datatype **Ids** ;
27. **Ids →** Identifier **Ids'** | Assignment_Statement **Ids'**
28. **Ids' →** , Identifier **Ids'** | Assignment_Statement **Ids'** | e
29. Datatype → int | float | string
30. Assignment_Statement → Identifier := Expression 
31. Expression → String | Term | Equation
32. Equation → **Equa** | ( **Equa** ) **eq**
33. **Equa →** Term **eq**
34. **eq →** Arithmetic_Op Term **eq** | e
35. Arithmatic_Op → + | - | * | /
36. Boolean_Op → && | ||
37. Condition_Op → < | > | = | <>
38. Term → Number | Identifier | Function_Call
39. Function_Call → Function_Name ( **Identifiers** )
40. **Identifiers →** Identifier **Identifier'** | e
41. **Identifier' → ,** Identifier **Identifier'** | e