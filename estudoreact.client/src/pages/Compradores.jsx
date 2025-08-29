import React, { useState, useEffect } from 'react';

function Compradores() {
    const [compradores, setCompradores] = useState([]);
    const [novoComprador, setNovoComprador] = useState({ id: null, nome: '', cidade: '' });
    const [modoEdicao, setModoEdicao] = useState(false);

    const API_URL = 'https://localhost:7048' ; // Altere para sua API real

    // Carregar compradores da API
    useEffect(() => {
        fetch(API_URL + '/Comprador', { method: 'GET' })
            .then((res) => res.json())
            .then((data) => setCompradores(data))
            .catch((err) => console.error('Erro ao carregar compradores:', err));
    }, []);

    // Atualiza campos de formulário
    const handleChange = (e) => {
        setNovoComprador({ ...novoComprador, [e.target.name]: e.target.value });
    };

    // Adicionar novo comprador
    const adicionarComprador = () => {
        if (!novoComprador.nome || !novoComprador.cidade) return;

        fetch(API_URL, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(novoComprador),
        })
            .then((res) => res.json())
            .then((data) => {
                setCompradores([...compradores, data]);
                setNovoComprador({ id: null, nome: '', cidade: '' });
            });
    };

    // Editar comprador (preencher formulário)
    const iniciarEdicao = (comprador) => {
        setModoEdicao(true);
        setNovoComprador(comprador);
    };

    // Salvar edição
    const salvarEdicao = () => {
        fetch(`${API_URL}/${novoComprador.id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(novoComprador),
        })
            .then((res) => res.json())
            .then((dataAtualizada) => {
                setCompradores(
                    compradores.map((c) => (c.id === dataAtualizada.id ? dataAtualizada : c))
                );
                setModoEdicao(false);
                setNovoComprador({ id: null, nome: '', cidade: '' });
            });
    };

    return (
        <div style={{ padding: '20px', fontFamily: 'Arial' }}>
            <h2>📋 Lista de Compradores</h2>

            <table border="1" cellPadding="8" cellSpacing="0">
                <thead>
                    <tr>
                        <th>Código</th>
                        <th>Nome</th>
                        <th>Cidade</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody>
                    {compradores.map((c) => (
                        <tr key={c.id}>
                            <td>{c.id}</td>
                            <td>{c.nome}</td>
                            <td>{c.cidade}</td>
                            <td>
                                <button onClick={() => iniciarEdicao(c)}>Editar</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <h3 style={{ marginTop: '30px' }}>
                {modoEdicao ? '✏️ Editar Comprador' : '➕ Adicionar Novo Comprador'}
            </h3>

            <div style={{ marginBottom: '10px' }}>
                <input
                    type="text"
                    name="nome"
                    placeholder="Nome"
                    value={novoComprador.nome}
                    onChange={handleChange}
                    style={{ marginRight: '10px' }}
                />
                <input
                    type="text"
                    name="cidade"
                    placeholder="Cidade"
                    value={novoComprador.cidade}
                    onChange={handleChange}
                />
            </div>

            <button onClick={modoEdicao ? salvarEdicao : adicionarComprador}>
                {modoEdicao ? 'Salvar' : 'Adicionar'}
            </button>
        </div>
    );
}

export default Compradores;
