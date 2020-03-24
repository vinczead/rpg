grammar ViGaS;

script: (baseDefinition | instanceDefinition)*;

baseDefinition: baseHeader definitionBody;
baseHeader: DEFINE baseId FROM baseClass;

instanceDefinition: instanceHeader definitionBody;
instanceHeader: INSTANTIATE instanceId FROM baseId;

definitionBody: variablesBlock? initBlock runBlock*;

initBlock: INIT statementList END;

variablesBlock: VARIABLES variableDeclaration+ END;

runBlock: RUN eventTypeName statementList END;

statement: assignmentStatement | assignmentStatementBlock | ifStatement | whileStatement | repeatStatement | functionCallStatement | RETURN | COMMENT;

statementList: statement*;

expression
	: functionCallStatement									#funcExpression
	| path													#pathExpression
	| STRING												#stringExpression
	| REFERENCE												#refExpression
	| NUMBER												#numberExpression
	| BOOLEAN												#boolExpression
	| NULL													#nullExpression
	| NOT expression										#notExpression
	| left=expression multiplOperator right=expression		#multiplExpression
	| left=expression additiveOperator right=expression		#additiveExpression
	| left=expression logicalOperator right=expression		#logicalExpression
	| left=expression compOperator right=expression			#compExpression
	| '(' expression ')'									#parenExpression
	;

path: varPath | refPath;

refPath: REFERENCE ('.' varName)*;
varPath: varName ('.' varName)*;

functionParameterList: expression (',' expression)*;

variableDeclaration: varName IS typeName ((WITHVALUE expression SHARED?) | PARAMETER )?;
assignmentStatement: SET path TO expression;
assignmentStatementBlock: SET (path TO expression)+ END;
ifStatement: IF expression THEN statementList (elseStatement)? END;
elseStatement: ELSE statementList;
whileStatement: WHILE expression statementList REPEAT;
repeatStatement: REPEAT statementList WHILE expression;

functionCallStatement: path? '=>' functionName '(' functionParameterList? ')';

additiveOperator: PLUS | MINUS;
multiplOperator: MULT | DIV;

compOperator: LT | GT | LTE | GTE | EQ | NEQ;
logicalOperator: OR | AND | XOR;

//---------------------------IDS---------------------------

baseId: ID;
baseClass: ID;
instanceId: ID;
typeName: ID;
varName: ID;
eventTypeName: ID;
functionName: ID;

//---------------------------OPERATORS--------------------------- 

ARROW: '=>';

LT: '<';
GT: '>';
LTE: '<=';
GTE: '>=';
EQ: '=';
NEQ: '<>';

OR: 'Or';
AND: 'And';
NOT: 'Not';
XOR: 'Xor';

PLUS: '+';
MINUS: '-';
MULT: '*';
DIV: '/';

COMMENT: '//' (~[\r\n])* -> skip;

//---------------------------KEYWORDS---------------------------

DEFINE: 'Define';
INSTANTIATE: 'Instantiate';
FROM: 'From';

VARIABLES: 'Variables';
IS: 'Is';
WITHVALUE: 'WithValue';
PARAMETER: 'Parameter';
SHARED: 'Shared';

INIT: 'Initialization';

RUN: 'Run';
WHEN: 'When';

IF: 'If';
THEN: 'Then';
ELSE: 'Else';

WHILE: 'While';
REPEAT: 'Repeat';
BREAK: 'Break';

RETURN: 'Return';

SET: 'Set';
TO: 'To';

END: 'End';

BRACKET_OPEN: '(';
BRACKET_CLOSE: ')';
COMMA: ',';
DOT: '.';

NULL: 'Null';
BOOLEAN: 'True' | 'False';
STRING: '"' (~[\r\n])* '"';
NUMBER: [0-9]+ ('.' [0-9]+)?;
REFERENCE: '$' ID;

ID: [a-zA-Z][a-zA-Z0-9_]*;
WS: (' '| '\t' | '\n' | '\r') -> skip;