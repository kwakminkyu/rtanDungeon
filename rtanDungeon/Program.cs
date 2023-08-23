using System.Security.Cryptography.X509Certificates;

namespace rtanDungeon {
    internal class Program {
        private static Character player;
        private static Inventory inventory;

        static void Main(string[] args) {
            GameDataSetting();
            DisplayGameIntro();
        }

        static void GameDataSetting() {
            // 캐릭터 정보 세팅
            player = new Character("Chad", "전사", 1, 10, 5, 100, 1500);

            // 아이템 정보 세팅
            inventory = new Inventory(20);
            inventory.GetItem(new Item().SetItem("무쇠갑옷", "방어력", 5, "무쇠로 만들어져 튼튼한 갑옷입니다."));
            inventory.GetItem(new Item().SetItem("낡은 검", "공격력", 2, "쉽게 볼 수 있는 낡은 검 입니다."));
            inventory.GetItem(new Item().SetItem("강철 검", "공격력", 5, "튼튼한 강철로 된 검 입니다."));
            inventory.GetItem(new Item().SetItem("모피갑옷", "체력", 30, "입기만 해도 따뜻해지는 갑옷입니다."));
        }

        static void DisplayGameIntro() {
            Console.Clear();

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 전전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int input = CheckValidInput(1, 2);
            switch (input) {
                case 1:
                    DisplayMyInfo();
                    break;

                case 2:
                    DisplayInventory();
                    break;
            }
        }

        static void DisplayMyInfo() {
            Console.Clear();

            Console.WriteLine("상태보기");
            Console.WriteLine("캐릭터의 정보를 표시합니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level}");
            Console.WriteLine($"{player.Name}({player.Job})");
            if (player.plusAtk != 0) {
                Console.WriteLine($"공격력 :{player.Atk} (+{player.plusAtk})");
            } else {
                Console.WriteLine($"공격력 :{player.Atk}");
            }
            if (player.plusDef != 0) {
                Console.WriteLine($"방어력 :{player.Def} (+{player.plusDef})");
            } else {
                Console.WriteLine($"방어력 : {player.Def}");
            }
            if (player.plusHp != 0) {
                Console.WriteLine($"체력 :{player.Hp} (+{player.plusHp})");
            } else {
                Console.WriteLine($"체력 : {player.Hp}");
            }
            Console.WriteLine($"Gold : {player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");

            int input = CheckValidInput(0, 0);
            switch (input) {
                case 0:
                    DisplayGameIntro();
                    break;
            }
        }

        static void DisplayInventory() {
            Console.Clear();

            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            inventory.ShowInventory();
            Console.WriteLine();
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");

            int input = CheckValidInput(0, 1);
            switch (input) {
                case 0:
                    DisplayGameIntro();
                    break;
                case 1:
                    DisplayEquipment();
                    break;
            }
        }

        static void DisplayEquipment() {
            Console.Clear();

            Console.WriteLine("장착관리");
            Console.WriteLine("아이템을 장착, 해제 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            inventory.ShowEquipment();
            Console.WriteLine();
            Console.WriteLine("아이템을 장착, 해제 하시려면 아이템 번호를 입력해 주세요");
            Console.WriteLine("0. 나가기");

            bool itemCheck = true;
            while(itemCheck) {
                int input = CheckValidInput(0, inventory.items.Length);
                switch (input) {
                    case 0:
                        DisplayGameIntro();
                        break;
                    default:
                        if (inventory.items[input - 1].name == null) {
                            Console.WriteLine("잘못된 입력입니다.");
                            continue;
                        } else {
                            inventory.Equip(input);
                            itemCheck = false;
                            DisplayEquipment();
                            break;
                        }
                }
            }
        }

        static int CheckValidInput(int min, int max) {
            while (true) {
                string input = Console.ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess) {
                    if (ret >= min && ret <= max)
                        return ret;
                }

                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }


    public class Character {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Hp { get; set; }
        public int Gold { get; set; }

        public int plusAtk { get; set; }
        public int plusDef { get; set; }
        public int plusHp { get; set; }

        public Character(string name, string job, int level, int atk, int def, int hp, int gold) {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
        }

        public void StatusUp(Item[] items) {
            foreach (var item in items) {
                if (item.equip) {
                    if (item.ability == "공격력") {
                        plusAtk += item.stat;
                        Atk += plusAtk;
                    } else if (item.ability == "방어력") {
                        plusDef += item.stat;
                        Def += plusAtk;
                    } else if (item.ability == "체력") {
                        plusHp += item.stat;
                        Hp += plusAtk;
                    } else {
                        return;
                    }
                }
            }
        }
    }

    public struct Item {
        public string name;
        public string ability;
        public int stat;
        public string description;
        public bool equip;

        public Item SetItem(string name, string ability, int stat, string description) {
            if (ability == "공격력" || ability == "방어력" || ability == "체력") {
                this.name = name;
                this.ability = ability;
                this.stat = stat;
                this.description = description;
                this.equip = false;
                return this;
            } else {
                this.name = null;
                return this;
            }
        }
    }

    public class Inventory {
        public Item[] items { get; set; }
        private int cursor = 0;

        public Inventory(int capacity) {
            items = new Item[capacity];
        }
        public void ShowInventory() {
            foreach (Item item in items) { 
                if (item.name == null) {
                    Console.WriteLine($"-");
                } else {
                    Console.WriteLine($"- {item.name} | {item.ability} +{item.stat} | {item.description}");
                }
            }
        }

        public void ShowEquipment() {
            for (int i = 0; i < items.Length; i++) {
                Item item = items[i];
                if (item.name != null) {
                    if (item.equip == true) {
                        Console.WriteLine($"{i + 1}. [E] {item.name} | {item.ability} +{item.stat} | {item.description}");
                    } else {
                        Console.WriteLine($"{i + 1}. {item.name} | {item.ability} +{item.stat} | {item.description}");
                    }
                }
            }
        }

        public void Equip(int number) {
            int num = number - 1;
            if (items[num].equip) {
                items[num].equip = false;
            } else {
                items[num].equip = true;
            }
        }

        public void GetItem(Item item) {
            if (item.name == null) {
                return;
            }
            if (items[cursor].name != null || cursor >= items.Length) {
                for (int i = 0; i < items.Length; i++) {
                    if (items[i].name == null) {
                        cursor = i;
                        break;
                    }
                }
            }
            items[cursor] = item;
            cursor++;
        }
    }
}