using Canais.Domain.Contracts.Repositories;
using Canais.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Canais.Infrastructure.Repositories;

public class ReclamacaoRepository : IReclamacaoRepository
{
    private readonly CanaisDbContext _context;

    public ReclamacaoRepository(CanaisDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReclamacoesEntity>> ListarReclamacoesClassificadasAsync()
    {
        var result = await _context.Reclamacoes
                                   .Include(r => r.ReclamacaoCategorias)      
                                   .ThenInclude(rc => rc.Categorias)      
                                   .AsNoTracking()                             
                                   .ToListAsync();

        return result;
    }

    public async Task<ReclamacoesEntity> CadastrarReclamacaoAsync(ReclamacoesEntity reclamacao)
    {
        await _context.Reclamacoes.AddAsync(reclamacao);

        await _context.SaveChangesAsync();

        return reclamacao;
    }

    public async Task AtualizarAnexosAsync(Guid id, List<string> anexos)
    {
        var reclamacao = await _context.Reclamacoes.FindAsync(id);
        if (reclamacao == null) return;

        reclamacao.Anexos.AddRange(anexos);

        _context.Entry(reclamacao).Property(r => r.Anexos).IsModified = true;
        await _context.SaveChangesAsync();
    }

    public async Task<ReclamacoesEntity> ObterPorIdAsync(Guid id)
    {
        var reclamacao = await _context.Reclamacoes
            .Include(r => r.ReclamacaoCategorias) 
            .FirstOrDefaultAsync(r => r.Id == id);

        return reclamacao;
    }
}
