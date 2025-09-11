using System.Text.Json;
using System.Text.Json.Serialization;
using Integration.Domain.Enums;

namespace Integration.Api.Converters
{
    public class ServicoAgendamentoConverter : JsonConverter<ServicoAgendamento>
    {
        public override ServicoAgendamento Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                
                // Tenta converter por nome (case insensitive)
                if (Enum.TryParse<ServicoAgendamento>(value, true, out var result))
                {
                    return result;
                }
                
                // Fallback para valores específicos
                return value?.ToLower() switch
                {
                    "consulta inicial" => ServicoAgendamento.ConsultaInicial,
                    "consultainicial" => ServicoAgendamento.ConsultaInicial,
                    "limpeza" => ServicoAgendamento.Limpeza,
                    "obturação" => ServicoAgendamento.Obturacao,
                    "obturacao" => ServicoAgendamento.Obturacao,
                    "tratamento canal" => ServicoAgendamento.TratamentoCanal,
                    "tratamentocanal" => ServicoAgendamento.TratamentoCanal,
                    "ortodontia" => ServicoAgendamento.Ortodontia,
                    "implante" => ServicoAgendamento.Implante,
                    "manutenção aparelho" => ServicoAgendamento.ManutencaoAparelho,
                    "manutencaoaparelho" => ServicoAgendamento.ManutencaoAparelho,
                    "consulta ortodôntica" => ServicoAgendamento.ConsultaOrtodontica,
                    "consulta ortodontica" => ServicoAgendamento.ConsultaOrtodontica,
                    "consultaortodontica" => ServicoAgendamento.ConsultaOrtodontica,
                    "outros" => ServicoAgendamento.Outros,
                    _ => throw new JsonException($"Valor '{value}' não é válido para ServicoAgendamento. Valores aceitos: ConsultaInicial, Limpeza, Obturacao, TratamentoCanal, Ortodontia, Implante, ManutencaoAparelho, ConsultaOrtodontica, Outros")
                };
            }
            
            if (reader.TokenType == JsonTokenType.Number)
            {
                var value = reader.GetInt32();
                if (Enum.IsDefined(typeof(ServicoAgendamento), value))
                {
                    return (ServicoAgendamento)value;
                }
                throw new JsonException($"Valor numérico '{value}' não é válido para ServicoAgendamento");
            }

            throw new JsonException($"Não é possível converter {reader.TokenType} para ServicoAgendamento");
        }

        public override void Write(Utf8JsonWriter writer, ServicoAgendamento value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
