import React, { useState, useEffect } from 'react';

function Produtos() {
    const [produtos, setProdutos] = useState([]);
    const [carregando, setCarregando] = useState(false);
    const [mensagem, setMensagem] = useState('');
    const [erro, setErro] = useState('');

    const [novoProduto, setNovoProduto] = useState({
        id: 0,
        nome: '',
        origem: ''
    });

    const [modoEdicao, setModoEdicao] = useState(false);

    const API_URL = 'https://localhost:7048/api';

    const OrigemEnum = {
        1: 'Bovina',
        2: 'Suína',
        3: 'Aves',
        4: 'Peixes'
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

    const buscarProdutos = () => {
        setCarregando(true);
        fetch(API_URL + '/Produto')
            .then(res => res.json())
            .then(data => setProdutos(data))
            .catch(err => exibirMensagem('Erro ao carregar produtos. ' + err, 'erro'))
            .finally(() => setCarregando(false));
    };

    useEffect(() => {
        buscarProdutos();
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;

        setNovoProduto(prev => ({
            ...prev,
            [name]: name === 'origem' ? parseInt(value) : value
        }));
    };

    const adicionarProduto = () => {
        if (!novoProduto.nome || !novoProduto.origem) {
            exibirMensagem('Por favor, preencha todos os campos.', 'erro');
            return;
        }

        setCarregando(true);
        fetch(API_URL + '/Produto', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(novoProduto),
        })
            .then(res => res.json())
            .then(data => {
                setProdutos([...produtos, data]);
                resetForm();
                exibirMensagem('Produto adicionado com sucesso!');
            })
            .catch(err => {
                exibirMensagem('Erro ao adicionar produto. ' + (err.message || 'Erro desconhecido'), 'erro');
            })
            .finally(() => setCarregando(false));
    };

    const iniciarEdicao = (produto) => {
        setModoEdicao(true);
        setNovoProduto({ ...produto });
    };

    const salvarEdicao = () => {
        if (!novoProduto.nome || !novoProduto.origem) {
            exibirMensagem('Por favor, preencha todos os campos.', 'erro');
            return;
        }

        setCarregando(true);
        fetch(API_URL + '/Produto/' + novoProduto.id, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(novoProduto),
        })
            .then(res => res.json())
            .then(dataAtualizada => {
                setProdutos(produtos.map(p => p.id === dataAtualizada.id ? dataAtualizada : p));
                resetForm();
                exibirMensagem('Produto atualizado com sucesso!');
            })
            .catch(err => exibirMensagem('Erro ao salvar edição. ' + err, 'erro'))
            .finally(() => setCarregando(false));
    };

    const iniciarExclusao = (produto) => {
        if (window.confirm(`Deseja realmente excluir o produto ${produto.nome}?`)) {
            setCarregando(true);
            fetch(API_URL + '/Produto/' + produto.id, {
                method: 'DELETE'
            })
                .then(() => {
                    setProdutos(produtos.filter(p => p.id !== produto.id));
                    exibirMensagem('Produto excluído com sucesso!');
                })
                .catch(err => exibirMensagem('Erro ao excluir produto. ' + err, 'erro'))
                .finally(() => setCarregando(false));
        }
    };

    const resetForm = () => {
        setModoEdicao(false);
        setNovoProduto({ id: 0, nome: '', origem: '' });
    };

    return (
        <div style={{ padding: '20px', fontFamily: 'Arial' }}>
            <h2>📦 Lista de Produtos</h2>

            {carregando && <div style={{ color: 'blue', marginBottom: '10px' }}>Carregando...</div>}
            {mensagem && <div style={{ color: 'green', marginBottom: '10px' }}>{mensagem}</div>}
            {erro && <div style={{ color: 'red', marginBottom: '10px' }}>{erro}</div>}

            <table border="1" cellPadding="8" cellSpacing="0">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Nome</th>
                        <th>Origem</th>
                        <th>Alterar</th>
                        <th>Excluir</th>
                    </tr>
                </thead>
                <tbody>
                    {produtos.map((p) => (
                        <tr key={p.id}>
                            <td>{p.id}</td>
                            <td>{p.nome}</td>
                            <td>{OrigemEnum[p.origem]}</td>
                            <td>
                                <button onClick={() => iniciarEdicao(p)} disabled={carregando}>Editar</button>
                            </td>
                            <td>
                                <button onClick={() => iniciarExclusao(p)} disabled={carregando}>Excluir</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <h3 style={{ marginTop: '30px' }}>
                {modoEdicao ? '✏️ Editar Produto' : '➕ Adicionar Novo Produto'}
            </h3>

            <div style={{ marginBottom: '10px' }}>
                <div>
                    <label htmlFor="nome">Nome:</label><br />
                <input
                    type="text"
                    name="nome"
                    placeholder="Nome do produto"
                    value={novoProduto.nome}
                    onChange={handleChange}
                    style={{ marginRight: '10px' }}
                    disabled={carregando}
                    />
                </div>
                <div>
                    <label htmlFor="origem">Origem:</label><br />
                <select
                    name="origem"
                    value={novoProduto.origem}
                    onChange={handleChange}
                    disabled={carregando}
                >
                    <option value="">Selecione a origem</option>
                    {Object.entries(OrigemEnum).map(([valor, texto]) => (
                        <option key={valor} value={valor}>
                            {texto}
                        </option>
                    ))}
                    </select>
                </div>
            </div>

            <button onClick={modoEdicao ? salvarEdicao : adicionarProduto} disabled={carregando}>
                {modoEdicao ? 'Salvar' : 'Adicionar'}
            </button>

            {modoEdicao && (
                <button onClick={resetForm} style={{ marginLeft: '10px' }} disabled={carregando}>
                    Cancelar
                </button>
            )}
        </div>
    );
}

export default Produtos;
