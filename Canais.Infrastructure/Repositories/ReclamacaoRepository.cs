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

    public async Task<IEnumerable<ReclamacoesEntity>> ListarReclamacoesClassificadasAsync(FiltroReclamacoesEntity request)
    {
        var page = request.Page ?? 1;
        var pageSize = request.PageSize ?? 10;
        var skip = (page - 1) * pageSize;

        var query = _context.Reclamacoes
                        .Include(r => r.ReclamacaoCategorias)
                        .ThenInclude(rc => rc.Categorias)
                        .AsNoTracking();

        query = ContruirFiltro(request, query);

        var result = await query
                            .Skip(skip)
                            .Take(pageSize)
                            .ToListAsync();

        return result;
    }

    public async Task<ReclamacoesEntity> CadastrarReclamacaoAsync(ReclamacoesEntity reclamacao)
    {
        await _context.Reclamacoes.AddAsync(reclamacao);

        await _context.SaveChangesAsync();

        return reclamacao;
    }

    public async Task AtualizarAnexosAsync(int id, List<string> anexos)
    {
        var reclamacao = await _context.Reclamacoes.FindAsync(id);
        if (reclamacao == null) return;

        reclamacao.Anexos.AddRange(anexos);

        _context.Entry(reclamacao).Property(r => r.Anexos).IsModified = true;
        await _context.SaveChangesAsync();
    }

    public async Task<ReclamacoesEntity> ObterPorIdAsync(int id)
    {
        var reclamacao = await _context.Reclamacoes
            .Include(r => r.ReclamacaoCategorias) 
            .FirstOrDefaultAsync(r => r.IdReclamacao == id);

        return reclamacao;
    }

    private IQueryable<ReclamacoesEntity> ContruirFiltro(FiltroReclamacoesEntity request, IQueryable<ReclamacoesEntity> query)
    {
        if (request.IdReclamacao != null)
            query = query.Where(r => r.IdReclamacao == request.IdReclamacao);

        if (!string.IsNullOrEmpty(request.CpfReclamante))
            query = query.Where(r => r.Cpf == request.CpfReclamante);

        if (request.DataInicio.HasValue)
            query = query.Where(r => r.DataAbertura >= request.DataInicio.Value);

        if (request.DataFim.HasValue)
            query = query.Where(r => r.DataAbertura <= request.DataFim.Value);

        if (request.ReclamacaoAtendida.HasValue)
            query = query.Where(r => r.Atendida == request.ReclamacaoAtendida);

        if (!string.IsNullOrEmpty(request.Categoria))
            query = query.Where(r => r.ReclamacaoCategorias.Any(rc => rc.Categorias.Nome == request.Categoria));


        return query;
    }
}
