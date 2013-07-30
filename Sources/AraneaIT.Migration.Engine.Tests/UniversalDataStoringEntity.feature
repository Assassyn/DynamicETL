Feature: Accessing data
	In order to access and convert data properly
	As a user of entity
	I want to be able to set a value as string and get it as diffrent value

@mytag
Scenario: Create entity and populate it with data
	Given I have dynamic Entity
	When I add "System.String" value named "name" equals to "test"
	And I add "System.Int32" value named "order" equals to "1"
	Then the result entity exists
	And the result entity has property called "name" of type "System.String" with value "test"
	And the result entity has property called "order" of type "System.Int32" with value "1"

Scenario: Access not existing property
	Given I have dynamic Entity
	Then the result entity exists
	And the result entity has property called "not-existing" of type "System.String" with value ""
