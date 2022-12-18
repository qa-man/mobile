Feature: Example

@001
@Example
Scenario Outline: Example
	Given Example '<day>'
	When Example
	Then Example '<status>'
	And Example '<check>'
Examples: 
| day       | status       | check |
| Tomorrow  | Nex day      | true  |
| Yesterday | Previous day | false |