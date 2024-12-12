import json
import enum

class operator(enum.Enum):
    add = 1
    sub = 2,
    mul = 3
    truediv = 4
    floordiv = 5
    mod = 6
    pow = 7
    lt = 8
    le = 9
    eq = 10
    ne = 11
    ge = 12
    gt = 13

def basic_calculator(input_str):
    """
    Perform a numeric operation on two numbers based on the input string.

    Parameters:
    input_str (str): A JSON string representing a dictionary with keys 'num1', 'num2', and 'operation'. Example: '{"num1": 5, "num2": 3, "operation": "add"}' or "{'num1': 67869, 'num2': 9030393, 'operation': 'divide'}"

    Returns:
    str: The formatted result of the operation.

    Raises:
    Exception: If an error occurs during the operation (e.g., division by zero).
    ValueError: If an unsupported operation is requested or input is invalid.
    """
    # Clean and parse the input string
    try:
        # Replace single quotes with double quotes
        input_str_clean = input_str.replace("'", "\"")
        # Remove any extraneous characters such as trailing quotes
        input_str_clean = input_str_clean.strip().strip("\"")   

        input_dict = json.loads(input_str_clean)
        num1 = input_dict['num1']
        num2 = input_dict['num2']
        operation = input_dict['operation']
    except (json.JSONDecodeError, KeyError) as e:
        return str(e), "Invalid input format. Please provide a valid JSON string."

    # Define the supported operations
    operations = {
        'add': operator.add,
        'subtract': operator.sub,
        'multiply': operator.mul,
        'divide': operator.truediv,
        'floor_divide': operator.floordiv,
        'modulus': operator.mod,
        'power': operator.pow,
        'lt': operator.lt,
        'le': operator.le,
        'eq': operator.eq,
        'ne': operator.ne,
        'ge': operator.ge,
        'gt': operator.gt
    }

    # Check if the operation is supported
    if operation in operations:
        try:
            # Perform the operation
            result = operations[operation](num1, num2)
            result_formatted = f"\n\nThe answer is: {result}.\nCalculated with basic_calculator."
            return result_formatted
        except Exception as e:
            return str(e), "\n\nError during operation execution."
    else:
        return "\n\nUnsupported operation. Please provide a valid operation."

def reverse_string(input_string):
    """
    Reverse the given string.

    Parameters:
    input_string (str): The string to be reversed.

    Returns:
    str: The reversed string.
    """
    # Reverse the string using slicing
    reversed_string = input_string[::-1]

    reversed_string = f"The reversed string is: {reversed_string}\n\n.Executed using the reverse_string function."
    # print (f"DEBUG: reversed_string: {reversed_string}")
    return reversed_string

def code_executor(input_string: str):
    cleaned = input_string.replace("\\n", "")
    eval(input_string)

class ToolBox:
    def __init__(self):
        self.tools_dict = {}

    def store(self, functions_list):
        """
        Stores the literal name and docstring of each function in the list.

        Parameters:
        functions_list (list): List of function objects to store.

        Returns:
        dict: Dictionary with function names as keys and their docstrings as values.
        """
        for func in functions_list:
            self.tools_dict[func["name"]] = func["doc"]
        return self.tools_dict

    def tools(self):
        """
        Returns the dictionary created in store as a text string.

        Returns:
        str: Dictionary of stored functions and their docstrings as a text string.
        """
        tools_str = ""
        for name, doc in self.tools_dict.items():
            tools_str += f"{name}: \"{doc}\"\n"
        return tools_str.strip()