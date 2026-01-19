import argparse
from random import randint
from typing import List, Callable


class Arguments:
    def __init__(
            self,
            num: int,
            bar: bool,
            ):
        self.num: int = num
        self.bar: bool = bar


def parseargs() -> Arguments:
    parser = argparse.ArgumentParser()
    parser.add_argument("--num", "-n", type=int, default=1)
    parser.add_argument("--bar", "-b", type='store_true')
    args = parser.parse_args()
    return Arguments(args.num, args.bar)


def __gen(l: List[str]) -> str:
    return l[randint(0, len(l) - 1)]


def noun() -> str:
    nouns = [
        "Electron",
        "Positron",
        "Proton",
        "Antiproton",
        "Jet",
        "Fuel",
        "Generator",
        "Vent",
        "Tower",
        "Net",
        "Power",
        "Octane",
        "Alkane",
        "Heptane",
        "Hexane",
        "Oil",
        "Gun",
        "Boat",
        "Fighter",
        "Rail",
        "Canon",
        "Blast",
        "Rocket",
        "Missile",
        "Lance",
        "Vault",
        "Labyrinth",
        "Storm",
        "Thunder",
        "Lightning"
    ]
    return __gen(nouns)


def conn() -> str:
    words = [
        "an",
        "a",
        "of",
        "the",
        "for",
        "from",
        "von",
        "to",
        "too",
        "at",
        "or",
        "and",
        "with",
        "by"
    ]
    return __gen(words)


__colors = [
    "Red",
    "Yellow",
    "Blue",
    "Green",
    "Orange",
    "Cyan",
    "Violet",
    "Black",
    "White",
    "Silver",
    "Gold",
    "Copper"
]


def color() -> str:
    return __gen(__colors)


def adj() -> str:
    adjs = [
        "Sonic",
        "Electro",
        "Super",
        "Mega",
        "Ultra",
        "Macro",
        "Rapid",
        "Flying",
        "Bright",
        "Dark",
        "Glowing"
    ]
    adjs += __colors
    return __gen(adjs)


def __resolve(l: List[Callable[[], str]]) -> str:
    s = []
    for item in l:
        s.append(item())
    return " ".join(s) 


def generate_bar() -> str:
    forms = [
        [noun],
        [adj, noun],
        [noun, adj, noun],
        [noun, conn, adj, noun]
    ]
    form = randint(0, len(forms) - 1)
    return __resolve(forms[form])


def main() -> None:
    # args = parseargs()
    if True: # args.bar:
        print(generate_bar())


if __name__ == "__main__":
    main()
