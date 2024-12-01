Feature: Supporter signs up

    Scenario: Supporter signs up successfully
        Given the supporter's salutation is Mrs
        And the supporter's first name is Jodie
        And the supporter's last name is Medhurst
        And the supporter's email address is Jodie.Medhurst77@hotmail.com
        When the supporter signs up
        Then there are no errors
        And Jodie is in the list of supporting people

    Scenario: Fan group signs up successfully
        Given the supporter's salutation is Mrs
        And the supporter's first name is Jodie
        And the supporter's last name is Medhurst
        And the supporter's email address is Jodie.Medhurst77@hotmail.com
        And the supporter signs up
        And the supporter's salutation is Captain
        And the supporter's first name is Jean-Luc
        And the supporter's last name is Picard
        And the supporter's email address is jlpic@aol.com
        And the supporter signs up
        And the supporter's salutation is Admiral
        And the supporter's first name is Benjamin
        And the supporter's last name is Sisko
        And the supporter's email address is benni@sisko.de
        When the supporter signs up
        Then there are no errors
        And Jodie is in the list of supporting people
        And Jean-Luc is in the list of supporting people
        And Benjamin is in the list of supporting people

    Scenario: Supporter (ID'd by email) cannot sinup twice even if that is a nice gesture
        Given the supporter's salutation is Mrs
        And the supporter's first name is Jodie
        And the supporter's last name is Medhurst
        And the supporter's email address is Jodie.Medhurst77@hotmail.com
        And the supporter signs up
        And the supporter's salutation is Mr
        And the supporter's first name is Joler
        And the supporter's last name is Mettwurst
        And the supporter's email address is Jodie.Medhurst77@hotmail.com
        When the supporter signs up
        Then field EmailAddress has the error: Each email address can only signup once.
        But Jodie is in the list of supporting people

    Scenario: Supporter enters no data
        When the supporter signs up
        Then nobody has signed up yet
        And there are the errors:
          | Field        | Error                                     |
          | FirstName    | First name must be at least 2 characters. |
          | LastName     | Last name must be at least 2 characters.  |
          | EmailAddress | Email address is not valid.               |

    Scenario: First name is too long
        Given the supporter's first name is x23456789112345678921
        When the supporter signs up
        Then nobody has signed up yet
        And field FirstName has the error: First name must be at most 20 characters.

    Scenario: Last name is too long
        Given the supporter's last name is x23456789112345678921
        When the supporter signs up
        Then nobody has signed up yet
        And field LastName has the error: Last name must be at most 20 characters.

    Scenario: Email address is too long
        Given the supporter's email address is x23456789112345678921245678931234567894@aol.com
        When the supporter signs up
        Then nobody has signed up yet
        And field EmailAddress has the error: Email address must be at most 40 characters.

    Scenario Outline: Email address is invalid
        Given the supporter's email address is <Email>
        When the supporter signs up
        Then nobody has signed up yet
        And field EmailAddress has the error: Email address is not valid.
        Examples:
        | Email    |
        | invalid  |
        | i@i.i    |
        | inv@alid |
        | inva.lid |

