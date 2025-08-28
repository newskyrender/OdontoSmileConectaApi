using AutoMapper;
using Integration.Domain.Enums;

namespace Integration.Service.AutoMapper
{
    // Profile específico para conversões de enum personalizadas
    public class EnumConverterProfile : Profile
    {
        public EnumConverterProfile()
        {
            // Conversores customizados para enums com nomes específicos do MySQL
            CreateMap<EstadoCivil, string>().ConvertUsing(src => ConvertEstadoCivilToMySql(src));
            CreateMap<string, EstadoCivil>().ConvertUsing(src => ConvertMySqlToEstadoCivil(src));

            CreateMap<Sexo, string>().ConvertUsing(src => ConvertSexoToMySql(src));
            CreateMap<string, Sexo>().ConvertUsing(src => ConvertMySqlToSexo(src));

            CreateMap<NumeroCadeiras, string>().ConvertUsing(src => ConvertNumeroCadeirasToMySql(src));
            CreateMap<string, NumeroCadeiras>().ConvertUsing(src => ConvertMySqlToNumeroCadeiras(src));

            CreateMap<TempoExperiencia, string>().ConvertUsing(src => ConvertTempoExperienciaToMySql(src));
            CreateMap<string, TempoExperiencia>().ConvertUsing(src => ConvertMySqlToTempoExperiencia(src));
        }

        private string ConvertEstadoCivilToMySql(EstadoCivil estadoCivil)
        {
            return estadoCivil switch
            {
                EstadoCivil.Solteiro => "solteiro",
                EstadoCivil.Casado => "casado",
                EstadoCivil.Divorciado => "divorciado",
                EstadoCivil.Viuvo => "viuvo",
                EstadoCivil.Outro => "outro",
                _ => "solteiro"
            };
        }

        private EstadoCivil ConvertMySqlToEstadoCivil(string value)
        {
            return value switch
            {
                "solteiro" => EstadoCivil.Solteiro,
                "casado" => EstadoCivil.Casado,
                "divorciado" => EstadoCivil.Divorciado,
                "viuvo" => EstadoCivil.Viuvo,
                "outro" => EstadoCivil.Outro,
                _ => EstadoCivil.Solteiro
            };
        }

        private string ConvertSexoToMySql(Sexo sexo)
        {
            return sexo switch
            {
                Sexo.Masculino => "masculino",
                Sexo.Feminino => "feminino",
                Sexo.Outro => "outro",
                Sexo.NaoInformar => "nao_informar",
                _ => "nao_informar"
            };
        }

        private Sexo ConvertMySqlToSexo(string value)
        {
            return value switch
            {
                "masculino" => Sexo.Masculino,
                "feminino" => Sexo.Feminino,
                "outro" => Sexo.Outro,
                "nao_informar" => Sexo.NaoInformar,
                _ => Sexo.NaoInformar
            };
        }

        private string ConvertNumeroCadeirasToMySql(NumeroCadeiras numeroCadeiras)
        {
            return numeroCadeiras switch
            {
                NumeroCadeiras.UmaCadeira => "1_cadeira",
                NumeroCadeiras.DuasCadeiras => "2_cadeiras",
                NumeroCadeiras.TresCadeiras => "3_cadeiras",
                NumeroCadeiras.QuatroCadeiras => "4_cadeiras",
                NumeroCadeiras.CincoOuMaisCadeiras => "5_mais_cadeiras",
                _ => "1_cadeira"
            };
        }

        private NumeroCadeiras ConvertMySqlToNumeroCadeiras(string value)
        {
            return value switch
            {
                "1_cadeira" => NumeroCadeiras.UmaCadeira,
                "2_cadeiras" => NumeroCadeiras.DuasCadeiras,
                "3_cadeiras" => NumeroCadeiras.TresCadeiras,
                "4_cadeiras" => NumeroCadeiras.QuatroCadeiras,
                "5_mais_cadeiras" => NumeroCadeiras.CincoOuMaisCadeiras,
                _ => NumeroCadeiras.UmaCadeira
            };
        }

        private string ConvertTempoExperienciaToMySql(TempoExperiencia tempoExperiencia)
        {
            return tempoExperiencia switch
            {
                TempoExperiencia.Menos1Ano => "menos_1_ano",
                TempoExperiencia.Entre1e5Anos => "1_5_anos",
                TempoExperiencia.Entre6e10Anos => "6_10_anos",
                TempoExperiencia.Entre11e20Anos => "11_20_anos",
                TempoExperiencia.Mais20Anos => "mais_20_anos",
                _ => "menos_1_ano"
            };
        }

        private TempoExperiencia ConvertMySqlToTempoExperiencia(string value)
        {
            return value switch
            {
                "menos_1_ano" => TempoExperiencia.Menos1Ano,
                "1_5_anos" => TempoExperiencia.Entre1e5Anos,
                "6_10_anos" => TempoExperiencia.Entre6e10Anos,
                "11_20_anos" => TempoExperiencia.Entre11e20Anos,
                "mais_20_anos" => TempoExperiencia.Mais20Anos,
                _ => TempoExperiencia.Menos1Ano
            };
        }
    }
}
