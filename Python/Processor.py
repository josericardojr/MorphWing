from kanren import Relation, facts, var, run
from Data.PlayerDamageData import *

key_enemy = ["KEYENEMY1", "KEYENEMY2", "KEYENEMY3", "KEYENEMY4"]
key_dif_multi = ["DIFMULTI1", "DIFMULTI2", "DIFMULTI3", "DIFMULTI4"]
enemy_difficult = [0, 0, 0, 0]
player_hit_rate = "PLAYERHITRATE"

balanceFactor = Relation()
difficultyAdjustMax = Relation()
difficultyAdjustMin = Relation()

damageBalanceFactor = 1.65

#infos retiradas do xml

#numero de hits em ordem de tipo de inimigo
hits = [0,0,0,0]
#numero de usos de ataque em ordem de tipo de inimigo
happenings = [0, 0, 0, 0]

difficultyMultipliers = [1, 1, 1, 1]
itemDistances = [0, 0, 0, 0]

#BASEAR EM TEMPO

#infos estabelecidas aqui
facts(balanceFactor, ("enemy1", "11"),
    ("enemy2", "10"),
    ("enemy3", "17"),
    ("enemy4", "9"))

facts(difficultyAdjustMin, ("enemy1", "-0.7"),
    ("enemy2", "-0.8"),
    ("enemy3", "-0.9"),
    ("enemy4", "-0.7"))

facts(difficultyAdjustMax, ("enemy1", "0.5"),
    ("enemy2", "0.32"),
    ("enemy3", "0.45"),
    ("enemy4", "0.5"))


def format_number(number):
    return round(float(number), 3)


def adjust_difficulty(factor):
    x = var()
    y = var()
    z = var()
    hap_factor = float(happenings[factor])
    hit_factor = float(hits[factor])
    bal_factor = run(1, x, balanceFactor("enemy{0}".format(factor + 1), x))
    min_adjust = run(1, y, difficultyAdjustMin("enemy{0}".format(factor + 1), y))
    max_adjust = run(1, z, difficultyAdjustMax("enemy{0}".format(factor + 1), z))

    if hap_factor > 0:
        result = min(max(0.05 * hap_factor - 0.05 * hit_factor * float(bal_factor[0]), float(min_adjust[0])), float(max_adjust[0]))
        enemy_difficult[factor] = format_number(result)
        print("{0}:{1};".format(key_enemy[factor], format_number(result)))


def adjust_player_damage (hit_time):
    result = hit_time * float(damageBalanceFactor)
    print("{0}:{1};".format(player_hit_rate, format_number(result)))


def return_pool_values(w1, w2, w3, w4, invert):

    value = (difficultyMultipliers[0] * w1) + (difficultyMultipliers[1] * w2) + (difficultyMultipliers[2] * w3) + (difficultyMultipliers[3] * w4)
    if invert:
        value = 5.1 - value

    return min(max(value, 0.7), 5.1)


def get_item_distances(m):

    for i in range(len(key_enemy)):
        difficultyMultipliers[i] = m[i]

    itemDistances[0] = return_pool_values(0.4, 1.2, 0.6, 0.4, False)
    itemDistances[1] = return_pool_values(0.4, 1.2, 0.6, 0.4, True)
    itemDistances[2] = return_pool_values(0.8, 1.3, 0.3, 0.3, False)
    itemDistances[3] = return_pool_values(0.8, 1.3, 0.3, 0.3, True)

    for i in range(len(key_enemy)):
        print("{0}:{1};".format(key_dif_multi[i], format_number(itemDistances[i])))
        

def get_xml_info(xml, args):
    vertexs = xml.vertexs()

    for v in vertexs:
        #if(v.label() == "Attacking (Enemy)"):
        #    print('id: {0}\ntype: {1}\nlabel: {2}\ndate: {3}'.format(v.id(), v.type(), v.label(), v.date()))

        if xml.vertexs()[v].label() == "Being Hit(Player)":
            if xml.vertexs()[v].attributes()[3].value() == "Straight":
                hits[0] += 1
            if xml.vertexs()[v].attributes()[3].value() == "Chaser":
                hits[1] += 1
            if xml.vertexs()[v].attributes()[3].value() == "Round":
                hits[2] += 1
            if xml.vertexs()[v].attributes()[3].value() == "Irregular":
                hits[3] += 1

        if xml.vertexs()[v].label() == "Attacking (Enemy)":
            if xml.vertexs()[v].attributes()[6].value() == "Enemy_Straight":
                happenings[0] += 1
            if xml.vertexs()[v].attributes()[6].value() == "Enemy_Chaser":
                happenings[1] += 1
            if xml.vertexs()[v].attributes()[6].value() == "Enemy_Round":
                happenings[2] += 1
            if xml.vertexs()[v].attributes()[6].value() == "Enemy_Irregular":
                happenings[3] += 1

    for j in range(len(key_enemy)):
        for i in range(len(args)):
            if key_enemy[j] in args[i]:
                adjust_difficulty(j)
   
    damage_data = DamageData(xml)
    adjust_player_damage(damage_data.result())

    get_item_distances(enemy_difficult)
