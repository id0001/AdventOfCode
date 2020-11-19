from utils.io import print_solution, read_by_line
import collections as col
import os

Instruction = col.namedtuple(
    "Instruction", ["source", "action", "input_wires", "output_wire"])

instructions = dict()


def resolve_wire(instruction_set, wire_buffer, wire):
    if type(wire) is str and not wire.isnumeric():

        # value is already resolved
        if wire in wire_buffer:
            return wire_buffer[wire]

        # value should be resolved
        instruction = instruction_set[wire]
        wire_buffer[wire] = instruction.action(
            instruction_set, wire_buffer, *instruction.input_wires)
        return wire_buffer[wire]
    else:
        return int(wire)


def execute_input(instruction_set, wire_buffer, wire_in):
    return resolve_wire(instruction_set, wire_buffer,  wire_in)


def execute_not(instruction_set, wire_buffer, wire_in):
    return ~resolve_wire(instruction_set, wire_buffer, wire_in) & 0xffff


def execute_and(instruction_set, wire_buffer, wire_in_a, wire_in_b):
    a = resolve_wire(instruction_set, wire_buffer, wire_in_a)
    b = resolve_wire(instruction_set, wire_buffer, wire_in_b)
    return a & b


def execute_or(instruction_set, wire_buffer, wire_in_a, wire_in_b):
    a = resolve_wire(instruction_set, wire_buffer, wire_in_a)
    b = resolve_wire(instruction_set, wire_buffer, wire_in_b)
    return a | b


def execute_lshift(instruction_set, wire_buffer, wire_in_a, wire_in_b):
    a = resolve_wire(instruction_set, wire_buffer, wire_in_a)
    b = resolve_wire(instruction_set,  wire_buffer, wire_in_b)
    return a << b


def execute_rshift(instruction_set,  wire_buffer, wire_in_a, wire_in_b):
    a = resolve_wire(instruction_set, wire_buffer, wire_in_a)
    b = resolve_wire(instruction_set, wire_buffer, wire_in_b)
    return a >> b


def create_input_instruction(line, wire_in, wire_out):
    return Instruction(line, execute_input, [wire_in], wire_out)


def create_not_instruction(line, wire_in, wire_out):
    return Instruction(line, execute_not, [wire_in],  wire_out)


def create_and_instruction(line, wire_in_a, wire_in_b, wire_out):
    return Instruction(line, execute_and, [wire_in_a, wire_in_b],  wire_out)


def create_or_instruction(line, wire_in_a, wire_in_b, wire_out):
    return Instruction(line, execute_or, [wire_in_a, wire_in_b],  wire_out)


def create_lshift_instruction(line, wire_in_a, wire_in_b, wire_out):
    return Instruction(line, execute_lshift, [wire_in_a, wire_in_b],  wire_out)


def create_rshift_instruction(line, wire_in_a, wire_in_b, wire_out):
    return Instruction(line, execute_rshift, [wire_in_a, wire_in_b],  wire_out)


for line in read_by_line(7):
    split = line.split()  # split on SPACE

    if len(split) == 3:  # INPUT
        instructions[split[2]] = create_input_instruction(
            line, split[0], split[2])
    elif len(split) == 4:  # NOT
        instructions[split[3]] = create_not_instruction(
            line, split[1], split[3])
    elif split[1] == 'AND':
        instructions[split[4]] = create_and_instruction(
            line, split[0], split[2], split[4])
    elif split[1] == 'OR':
        instructions[split[4]] = create_or_instruction(
            line, split[0], split[2], split[4])
    elif split[1] == 'LSHIFT':
        instructions[split[4]] = create_lshift_instruction(
            line, split[0], split[2], split[4])
    elif split[1] == 'RSHIFT':
        instructions[split[4]] = create_rshift_instruction(
            line, split[0], split[2], split[4])

wire_buffer = dict()
print_solution(resolve_wire(instructions, wire_buffer, 'a'))
