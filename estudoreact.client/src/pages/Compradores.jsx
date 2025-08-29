import React, { useState, useEffect } from 'react';

function Compradores() {
    const [compradores, setCompradores] = useState([]);
    const [estados, setEstados] = useState([]);
    const [cidades, setCidades] = useState([]);

    const [novoComprador, setNovoComprador] = useState({
        id: 0,
        nome: '',
        documento: '',
        idEstado: '',
        idCidade: '',
        estadoNome: '',
        cidadeNome: ''
    });

    const [modoEdicao, setModoEdicao] = useState(false);

    const API_URL = 'https://localhost:7048';

    useEffect(() => {
        // Buscar compradores
        fetch(API_URL + '/Comprador')
            .then(res => res.json())
            .then(data => setCompradores(data))
            .catch(err => console.error('Erro ao carregar compradores:', err));

        // Buscar estados
        fetch(API_URL + '/Estado')
            .then(res => res.json())
            .then(data => setEstados(data))
            .catch(err => console.error('Erro ao carregar estados:', err));
    }, []);

    useEffect(() => {
        if (novoComprador.idEstado) {
            fetch(API_URL + '/Cidade/' + novoComprador.idEstado + '/Cidade')
                .then(res => res.json())
                .then(data => setCidades(data))
                .catch(err => console.error('Erro ao carregar cidades:', err));
        } else {
            setCidades([]);
            setNovoComprador(prev => ({ ...prev, idCidade: '', cidadeNome: '' })); // Limpa cidade ao mudar estado
        }
    }, [novoComprador.idEstado]);

    const handleChange = (e) => {
        const { name, value } = e.target;

        if (name === 'idEstado') {
            // Quando muda o estado, atualizar idestado e estadonome
            const estadoSelecionado = estados.find(e => e.id === value);
            setNovoComprador(prev => ({
                ...prev,
                idEstado: value,
                estadoNome: estadoSelecionado ? estadoSelecionado.nome : '',
                idCidade: '',
                cidadeNome: '',
            }));
        } else if (name === 'idCidade') {
            // Quando muda a cidade, atualizar idcidade e cidadenome
            const cidadeSelecionada = cidades.find(c => c.id === value);
            setNovoComprador(prev => ({
                ...prev,
                idCidade: value,
                cidadeNome: cidadeSelecionada ? cidadeSelecionada.nome : '',
            }));
        } else {
            setNovoComprador(prev => ({
                ...prev,
                [name]: value
            }));
        }
    };

    const adicionarComprador = () => {
        if (!novoComprador.nome || !novoComprador.documento || !novoComprador.idEstado || !novoComprador.idEstado) {
            alert('Por favor, preencha todos os campos.');
            return;
        }

        fetch(API_URL + '/Comprador', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(novoComprador),
        })
            .then(res => res.json())
            .then(data => {
                setCompradores([...compradores, data]);
                resetForm();
            })
            .catch(err => console.error('Erro ao adicionar comprador:', err));
    };

    const iniciarEdicao = (comprador) => {
        setModoEdicao(true);
        setNovoComprador(comprador);
    };

    const salvarEdicao = () => {
        fetch(API_URL + '/Comprador/' + novoComprador.id, {  // Template string corrigida
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(novoComprador),
        })
            .then(res => res.json())
            .then((dataAtualizada) => {
                setCompradores(
                    compradores.map((c) => (c.id === dataAtualizada.id ? dataAtualizada : c))
                );
                resetForm();
            })
            .catch(err => console.error('Erro ao salvar edição:', err));
    };

    const resetForm = () => {
        setModoEdicao(false);
        setNovoComprador({ id: null, nome: '', documento: '', idestado: '', idCidade: '', estadonome: '', cidadeNome: '' });
        setCidades([]);
    };

    return (
        <div style={{ padding: '20px', fontFamily: 'Arial' }}>
            <h2>📋 Lista de Compradores</h2>

            <table border="1" cellPadding="8" cellSpacing="0">
                <thead>
                    <tr>
                        <th>Código</th>
                        <th>Nome</th>
                        <th>Documento</th>
                        <th>Estado</th>
                        <th>Cidade</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody>
                    {compradores.map((c) => (
                        <tr key={c.id}>
                            <td>{c.id}</td>
                            <td>{c.nome}</td>
                            <td>{c.documento}</td>
                            <td>{c.estadoNome}</td>
                            <td>{c.cidadeNome}</td>
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
                    name="documento"
                    placeholder="Documento"
                    value={novoComprador.documento}
                    onChange={handleChange}
                    style={{ marginRight: '10px' }}
                />
                <select
                    name="idEstado"
                    value={novoComprador.idEstado}
                    onChange={handleChange}
                    style={{ marginRight: '10px' }}
                >
                    <option value="">Selecione um estado</option>
                    {estados.map((uf) => (
                        <option key={uf.id} value={uf.id}>
                            {uf.nome}
                        </option>
                    ))}
                </select>
                <select
                    name="idCidade"
                    value={novoComprador.idCidade}
                    onChange={handleChange}
                >
                    <option value="">Selecione uma cidade</option>
                    {cidades.map((cidade) => (
                        <option key={cidade.id} value={cidade.id}>
                            {cidade.nome}
                        </option>
                    ))}
                </select>
            </div>

            <button onClick={modoEdicao ? salvarEdicao : adicionarComprador}>
                {modoEdicao ? 'Salvar' : 'Adicionar'}
            </button>

            {modoEdicao && (
                <button onClick={resetForm} style={{ marginLeft: '10px' }}>
                    Cancelar
                </button>
            )}
        </div>
    );
}

export default Compradores;
