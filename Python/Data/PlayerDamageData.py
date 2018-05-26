
class DamageData:
    def __init__(self, xml):
        vertexs = xml.vertexs()
        tag = 'Being Hit(Enemy)'
        aux_sum_att = 0
        aux_bigger_time = 0
        for v in vertexs:
            if tag in xml.vertexs()[v].label():
                aux_sum_att += 1
                if aux_bigger_time < float(xml.vertexs()[v].date()):
                    aux_bigger_time = float(xml.vertexs()[v].date())

        self.d_sum_att = aux_sum_att
        self.d_bigger_time = aux_bigger_time
        self.d_result = aux_sum_att / aux_bigger_time
        #print('sum_vert: {0}\nbigger_time: {1}\n{0}/{1} = {2}'.format(sum_att, bigger_time, self.d_result))

    def result(self):
        return self.d_result

    def sum_att(self):
        return self.d_sum_att

    def bigger_time(self):
        return self.d_bigger_time
