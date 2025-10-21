import React, { useEffect, useState } from 'react';

const MOEDAS = {
    1: 'Real',
    2: 'Dólar',
    3: 'Euro',
};

const API_URL = 'https://localhost:7048/api';
const COTACAO_API_BASE = 'https://economia.awesomeapi.com.br/json/last';

function Pedidos() {
    const [pedidos, setPedidos] = useState([]);
    const [compradores, setCompradores] = useState([]);
    const [produtos, setProdutos] = useState([]);

    const [carregando, setCarregando] = useState(false);
    const [mensagem, setMensagem] = useState('');
    const [erro, setErro] = useState('');
    const [modoEdicao, setModoEdicao] = useState(false);

    const [pedido, setPedido] = useState({
        id: 0,
        dataInclusao: '',
        idComprador: '',
        itens: [],
    });

    // estado para armazenar cotações de USD e EUR para BRL
    const [cotacoes, setCotacoes] = useState({
        USD: null,
        EUR: null,
    });

    const resetForm = () => {
        setModoEdicao(false);
        setPedido({
            id: 0,
            dataInclusao: '',
            idComprador: '',
            itens: [],
        });
    };

    const exibirMensagem = (texto, tipo = 'sucesso') => {
        if (tipo === 'erro') {
            setErro(texto);
            setTimeout(() => setErro(''), 4000);
        } else {
            setMensagem(texto);
            setTimeout(() => setMensagem(''), 4000);
        }
    };

    // Busca cotação para a moeda (USD ou EUR)
    const buscarCotacao = async (moedaCodigo) => {
        try {
            const url = `${COTACAO_API_BASE}/${moedaCodigo}-BRL`;
            const resp = await fetch(url);
            if (!resp.ok) {
                throw new Error(`Erro ao buscar cotação ${moedaCodigo}`);
            }
            const data = await resp.json();
            const chave = `${moedaCodigo}BRL`;
            const askString = data[chave]?.ask;
            if (!askString) {
                throw new Error('Campo ask não encontrado');
            }
            return parseFloat(askString);
        } catch (error) {
            console.error(error);
            exibirMensagem(`Falha ao obter cotação para ${moedaCodigo}`, 'erro');
            return null;
        }
    };

    // Carrega dados iniciais: pedidos, compradores, produtos e cotações
    const carregarDados = async () => {
        setCarregando(true);
        try {
            const [resPedidos, resCompradores, resProdutos] = await Promise.all([
                fetch(API_URL + '/Pedido'),
                fetch(API_URL + '/Comprador'),
                fetch(API_URL + '/Produto'),
            ]);

            const [pedidosData, compradoresData, produtosData] = await Promise.all([
                resPedidos.json(),
                resCompradores.json(),
                resProdutos.json(),
            ]);

            setPedidos(pedidosData);
            setCompradores(compradoresData);
            setProdutos(produtosData);

            // Carregar cotações uma única vez
            const [usd, eur] = await Promise.all([
                buscarCotacao('USD'),
                buscarCotacao('EUR'),
            ]);
            setCotacoes({ USD: usd, EUR: eur });
        } catch (err) {
            exibirMensagem('Erro ao carregar dados.', 'erro');
        } finally {
            setCarregando(false);
        }
    };

    useEffect(() => {
        carregarDados();
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setPedido(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const adicionarItem = () => {
        setPedido(prev => ({
            ...prev,
            itens: [
                ...prev.itens,
                {
                    idProduto: '',
                    valor: '',
                    moeda: '',
                },
            ]
        }));
    };

    const removerItem = (index) => {
        setPedido(prev => ({
            ...prev,
            itens: prev.itens.filter((_, i) => i !== index)
        }));
    };

    // Atualiza campo de item e busca cotação se necessário
    const handleItemChange = async (index, field, value) => {
        const itensAtualizados = [...pedido.itens];

        if (field === 'moeda') {
            // garante que armazenamos número (1, 2, 3) e não string
            itensAtualizados[index][field] = parseInt(value, 10);
        } else if (field === 'valor') {
            itensAtualizados[index][field] = value; // string aqui para input
        } else {
            itensAtualizados[index][field] = value;
        }

        setPedido(prev => ({
            ...prev,
            itens: itensAtualizados
        }));

        // Só buscar cotação se não estiver carregada e moeda for dólar ou euro
        if (field === 'moeda' && (value === '2' || value === '3')) {
            const codigoMoeda = (value === '2') ? 'USD' : 'EUR';
            if (!cotacoes[codigoMoeda]) {
                const cotacaoValor = await buscarCotacao(codigoMoeda);
                if (cotacaoValor !== null) {
                    setCotacoes(prev => ({
                        ...prev,
                        [codigoMoeda]: cotacaoValor
                    }));
                }
            }
        }
    };

    // Exibe a conversão do valor para Real (BRL)
    const exibirConversao = (valor, moeda) => {
        const valorNum = parseFloat(valor) || 0;
        const codigo = moeda === 2 ? 'USD' : 'EUR'; // aqui moeda é número
        const cotacaoAtual = cotacoes[codigo];

        if (cotacaoAtual) {
            const convertido = valorNum * cotacaoAtual;
            return <span style={{ marginLeft: 10 }}>≈ R$ {convertido.toFixed(2)}</span>;
        } else {
            return <span style={{ marginLeft: 10 }}>Buscando cotação...</span>;
        }
    };

    const salvarPedido = () => {
        if (!pedido.dataInclusao || !pedido.idComprador || pedido.itens.length === 0) {
            exibirMensagem('Preencha todos os campos obrigatórios.', 'erro');
            return;
        }

        if (pedido.itens.some(item => !item.idProduto || !item.valor || !item.moeda)) {
            exibirMensagem('Preencha todos os campos dos itens do pedido.', 'erro');
            return;
        }

        if (new Date(pedido.dataInclusao) > new Date()) {
            exibirMensagem('A data do pedido não pode ser futura.', 'erro');
            return;
        }

        const metodo = pedido.id === 0 ? 'POST' : 'PUT';
        const url = pedido.id === 0 ? API_URL + '/Pedido' : `${API_URL}/Pedido/${pedido.id}`;

        setCarregando(true);
        fetch(url, {
            method: metodo,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(pedido)
        })
            .then(async res => {
                if (!res.ok) {
                    const erroTexto = await res.text();
                    throw new Error(erroTexto);
                }
                return res.json();
            })
            .then(() => {
                exibirMensagem(pedido.id === 0 ? 'Pedido adicionado com sucesso!' : 'Pedido atualizado com sucesso!');
                resetForm();
                carregarDados();
            })
            .catch(err => exibirMensagem('Erro ao salvar pedido: ' + err.message, 'erro'))
            .finally(() => setCarregando(false));
    };

    const editarPedido = (pedidoParaEditar) => {
        setModoEdicao(true);
        setPedido({
            ...pedidoParaEditar,
            dataInclusao: pedidoParaEditar.dataInclusao.split('T')[0],
        });
    };
    const formatarData = (dataISO) => {
        if (!dataISO) return '';
        const data = new Date(dataISO);
        return new Intl.DateTimeFormat('pt-BR').format(data);
    };

    const excluirPedido = (id) => {
        if (!window.confirm('Deseja excluir este pedido?')) return;

        setCarregando(true);
        fetch(`${API_URL}/Pedido/${id}`, {
            method: 'DELETE'
        })
            .then(res => {
                if (!res.ok) throw new Error('Erro ao excluir');
                setPedidos(prev => prev.filter(p => p.id !== id));
                exibirMensagem('Pedido excluído com sucesso!');
            })
            .catch(err => exibirMensagem('Erro ao excluir pedido: ' + err.message, 'erro'))
            .finally(() => setCarregando(false));
    };

    return (
        <div style={{ padding: 20 }}>
            <h2>📦 Cadastro de Pedidos</h2>

            {carregando && <div style={{ color: 'blue' }}>Carregando...</div>}
            {mensagem && <div style={{ color: 'green' }}>{mensagem}</div>}
            {erro && <div style={{ color: 'red' }}>{erro}</div>}

            <table border="1" cellPadding="8" style={{ marginBottom: 30 }}>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Data</th>
                        <th>Comprador</th>
                        <th>Itens</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody>
                    {pedidos.map(p => (
                        <tr key={p.id}>
                            <td>{p.id}</td>
                            <td>{formatarData(p.dataInclusao)}</td>
                            <td>{compradores.find(c => c.id === p.idComprador)?.nome || '---'}</td>
                            <td>
                                <ul>
                                    {p.itens.map((item, idx) => {
                                        const nomeProduto = produtos.find(prod => prod.id === item.idProduto)?.nome || 'Produto';
                                        const moedaLabel = MOEDAS[item.moeda];
                                        let cdMoeda = '1';
                                        if (item.moeda === '2')
                                            cdMoeda = 'USD';
                                        else if (item.moeda === '3')
                                            cdMoeda = 'EUR';
                                        const valorOriginal = parseFloat(item.valor);
                                        return (
                                            <li key={idx}>
                                                {nomeProduto} - {valorOriginal.toFixed(2)} ({moedaLabel})
                                                {item.moeda !== 1 && cotacoes[cdMoeda] ? (
                                                    <> → R$ {(valorOriginal * cotacoes[cdMoeda]).toFixed(2)}</>
                                                ) : item.moeda !== 1 ? (
                                                    <> → (cotação indisponível)</>
                                                ) : null}
                                            </li>
                                        );
                                    })}
                                </ul>
                            </td>

                            <td>
                                <button onClick={() => editarPedido(p)}>Editar</button>
                                <button onClick={() => excluirPedido(p.id)} style={{ marginLeft: 10 }}>Excluir</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <h3>{modoEdicao ? '✏️ Editar Pedido' : '➕ Novo Pedido'}</h3>

            <div style={{ marginBottom: 10 }}>
                <label htmlFor="dataInclusao">Data do Pedido:</label>
                <input
                    type="date"
                    name="dataInclusao"
                    value={pedido.dataInclusao}
                    onChange={handleChange}
                    max={new Date().toISOString().split('T')[0]}
                    style={{ marginLeft: 10 }}
                />
            </div>

            <div style={{ marginBottom: 10 }}>
                <label htmlFor="idComprador">Comprador:</label>
                <select
                    name="idComprador"
                    value={pedido.idComprador}
                    onChange={handleChange}
                    style={{ marginLeft: 10 }}
                >
                    <option value="">Selecione</option>
                    {compradores.map(c => (
                        <option key={c.id} value={c.id}>{c.nome}</option>
                    ))}
                </select>
            </div>

            <div>
                <h4>Itens do Pedido</h4>
                <p>Itens adicionados: {pedido.itens.length}</p>

                {pedido.itens.map((item, index) => (
                    <div key={index} style={{ marginBottom: 10 }}>
                        <label htmlFor={`produto-${index}`}>Produto:</label>
                        <select
                            name="idProduto"
                            value={item.idProduto}
                            onChange={e => handleItemChange(index, 'idProduto', e.target.value)}
                            style={{ marginRight: 10 }}
                        >
                            <option value="">Produto</option>
                            {produtos.map(p => (
                                <option key={p.id} value={p.id}>{p.nome}</option>
                            ))}
                        </select>

                        <label htmlFor={`valor-${index}`}>Valor:</label>
                        <input
                            name="valor"
                            type="number"
                            step="0.01"
                            min="0"
                            placeholder="Valor"
                            value={item.valor}
                            onChange={e => handleItemChange(index, 'valor', e.target.value)}
                            style={{ width: '100px', marginRight: 10 }}
                        />

                        <label htmlFor={`moeda-${index}`}>Moeda:</label>
                        <select
                            name="moeda"
                            value={item.moeda}
                            onChange={e => handleItemChange(index, 'moeda', e.target.value)}
                            style={{ marginRight: 10 }}
                        >
                            <option value="">Selecione a moeda</option>
                            <option value="1">Real</option>
                            <option value="2">Dólar</option>
                            <option value="3">Euro</option>
                        </select>

                        {/* Exibir conversão automática se moeda for Dólar ou Euro */}
                        {item.valor && item.moeda && (item.moeda === 2 || item.moeda === 3) && exibirConversao(item.valor, item.moeda)}

                        <button onClick={() => removerItem(index)}>🗑️</button>
                    </div>
                ))}

                <button onClick={adicionarItem}>+ Adicionar Item</button>
            </div>

            <div style={{ marginTop: 20 }}>
                <button onClick={salvarPedido}>
                    {modoEdicao ? 'Salvar Alterações' : 'Cadastrar Pedido'}
                </button>
                {modoEdicao && (
                    <button onClick={resetForm} style={{ marginLeft: 10 }}>Cancelar</button>
                )}
            </div>
        </div>
    );
}

export default Pedidos;
