import React, { useState, useEffect } from 'react';

function Compradores() {
    const [compradores, setCompradores] = useState([]);
    const [estados, setEstados] = useState([]);
    const [cidades, setCidades] = useState([]);

    const [carregando, setCarregando] = useState(false);
    const [mensagem, setMensagem] = useState('');
    const [erro, setErro] = useState('');

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

    const API_URL = 'https://localhost:7048/api';

    const exibirMensagem = (texto, tipo = 'sucesso') => {
        if (tipo === 'erro') {
            setErro(texto);
            setTimeout(() => setErro(''), 4000);
        } else {
            setMensagem(texto);
            setTimeout(() => setMensagem(''), 4000);
        }
    };

    const buscarCompradores = () => {
        setCarregando(true);
        fetch(API_URL + '/Comprador')
            .then(res => res.json())
            .then(data => setCompradores(data))
            .catch(err => exibirMensagem('Erro ao carregar compradores.' + err, 'erro'))
            .finally(() => setCarregando(false));
    };

    useEffect(() => {
        buscarCompradores();

        fetch(API_URL + '/Estado')
            .then(res => res.json())
            .then(data => setEstados(data))
            .catch(err => exibirMensagem('Erro ao carregar estados.' + err, 'erro'));
    }, []);

    useEffect(() => {
        if (novoComprador.idEstado) {
            fetch(API_URL + '/Cidade/por-uf/' + novoComprador.idEstado)
                .then(res => res.json())
                .then(data => setCidades(data))
                .catch(err => exibirMensagem('Erro ao carregar cidades.' + err, 'erro'));
        } else {
            setCidades([]);
            setNovoComprador(prev => ({ ...prev, idCidade: '', cidadeNome: '' }));
        }
    }, [novoComprador.idEstado]);

    const handleChange = (e) => {
        const { name, value } = e.target;

        if (name === 'documento') {
            // Permitir apenas números
            const somenteNumeros = value.replace(/\D/g, '').slice(0, 14);
            setNovoComprador(prev => ({
                ...prev,
                [name]: somenteNumeros
            }));
            return;
        }

        if (name === 'idEstado') {
            const id = parseInt(value);
            const estadoSelecionado = estados.find(e => e.id === id);
            setNovoComprador(prev => ({
                ...prev,
                idEstado: id,
                estadoNome: estadoSelecionado ? estadoSelecionado.nome : '',
                idCidade: '',
                cidadeNome: '',
            }));
        } else if (name === 'idCidade') {
            const id = parseInt(value);
            const cidadeSelecionada = cidades.find(c => c.id === id);
            setNovoComprador(prev => ({
                ...prev,
                idCidade: id,
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
        if (!novoComprador.nome || !novoComprador.documento || !novoComprador.idEstado || !novoComprador.idCidade) {
            exibirMensagem('Por favor, preencha todos os campos.');
            return;
        }

        fetch(API_URL + '/Comprador', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(novoComprador),
        })
            .then(async (res) => {
                if (!res.ok) {
                    const errorText = await res.text(); // Lê o corpo da resposta como texto
                    throw new Error(errorText);
                }
                return res.json(); // Sucesso, converte para JSON
            })
            .then(data => {
                setCompradores([...compradores, data]);
                resetForm();
                exibirMensagem('Comprador adicionado com sucesso!');
            })
            .catch(err => {
                exibirMensagem('Erro ao adicionar comprador: ' + err.message, 'erro');
            });
    };

    const iniciarEdicao = (comprador) => {
        setModoEdicao(true);
        const estado = estados.find(e => e.id === comprador.idEstado);
        const cidade = cidades.find(c => c.id === comprador.idCidade);

        setNovoComprador({
            ...comprador,
            estadoNome: estado ? estado.nome : '',
            cidadeNome: cidade ? cidade.nome : ''
        });

        if (comprador.idEstado) {
            fetch(API_URL + '/Cidade/por-uf/' + comprador.idEstado)
                .then(res => res.json())
                .then(data => setCidades(data))
                .catch(err => exibirMensagem('Erro ao carregar cidades.' + err, 'erro'));
        } else {
            setCidades([]);
        }
    };

    const iniciarExclusao = (comprador) => {
        if (window.confirm(`Deseja realmente excluir o comprador ${comprador.nome}?`)) {
            setCarregando(true);
            fetch(API_URL + '/Comprador/' + comprador.id, {
                method: 'DELETE'
            })
                .then(() => {
                    setCompradores(compradores.filter(c => c.id !== comprador.id));
                    exibirMensagem('Comprador excluído com sucesso!');
                })
                .catch(err => exibirMensagem('Erro ao excluir comprador.' + err, 'erro'))
                .finally(() => setCarregando(false));
        }
    };

    const salvarEdicao = () => {
        if (!novoComprador.nome || !novoComprador.documento || !novoComprador.idEstado || !novoComprador.idCidade) {
            exibirMensagem('Por favor, preencha todos os campos.');
            return;
        }        
        const payload = {
            id: novoComprador.id,
            nome: novoComprador.nome,
            documento: novoComprador.documento,
            idEstado: parseInt(novoComprador.idEstado),
            idCidade: parseInt(novoComprador.idCidade)
        };

        fetch(API_URL + '/Comprador/' + novoComprador.id, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload),
        })
            .then(async (res) => {
                if (!res.ok) {
                    const errorText = await res.text(); // Lê o corpo da resposta como texto
                    throw new Error(errorText);
                }
                return res.json(); // Sucesso, converte para JSON
            })
            .then(data => {
                setCompradores([...compradores, data]);
                resetForm();
                exibirMensagem('Comprador adicionado com sucesso!');
            })
            .catch(err => {
                exibirMensagem('Erro ao adicionar comprador: ' + err.message, 'erro');
            });
    };

    const resetForm = () => {
        setModoEdicao(false);
        setNovoComprador({ id: 0, nome: '', documento: '', idEstado: '', idCidade: '', estadoNome: '', cidadeNome: '' });
        setCidades([]);
    };

    return (
        <div style={{ padding: '20px', fontFamily: 'Arial' }}>
            <h2>📋 Lista de Compradores</h2>

            {carregando && <div style={{ color: 'blue', marginBottom: '10px' }}>Carregando...</div>}
            {mensagem && <div style={{ color: 'green', marginBottom: '10px' }}>{mensagem}</div>}
            {erro && <div style={{ color: 'red', marginBottom: '10px' }}>{erro}</div>}

            <table border="1" cellPadding="8" cellSpacing="0">
                <thead>
                    <tr>
                        <th>Código</th>
                        <th>Nome</th>
                        <th>Documento</th>
                        <th>Estado</th>
                        <th>Cidade</th>
                        <th>Alterar</th>
                        <th>Excluir</th>
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
                                <button onClick={() => iniciarEdicao(c)} disabled={carregando}>Editar</button>
                            </td>
                            <td>
                                <button onClick={() => iniciarExclusao(c)} disabled={carregando}>Excluir</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <h3 style={{ marginTop: '30px' }}>
                {modoEdicao ? '✏️ Editar Comprador' : '➕ Adicionar Novo Comprador'}
            </h3>

            <div style={{ marginBottom: '10px' }}>
                <div>
                    <label htmlFor="nome">Nome:</label><br />
                    <input
                        type="text"
                        name="nome"
                        placeholder="Nome"
                        value={novoComprador.nome}
                        onChange={handleChange}
                        style={{ marginRight: '10px' }}
                        disabled={carregando}
                    />
                </div>
                <div>
                    <label htmlFor="documento">Documento:</label><br />
                    <input
                        type="text"
                        name="documento"
                        placeholder="Documento"
                        value={novoComprador.documento}
                        inputMode="numeric"
                        pattern="\d*"
                        onChange={handleChange}
                        maxLength={14}
                        style={{ marginRight: '10px' }}
                        disabled={carregando}
                        onPaste={(e) => {
                            // Pega o texto que o usuário tentou colar
                            const paste = e.clipboardData.getData('text');

                            // Verifica se o texto contém só números
                            if (!/^\d+$/.test(paste)) {
                                e.preventDefault(); // bloqueia o paste se tiver caracteres não numéricos
                            }
                        }}
                    />
                </div>
                <div>
                    <label htmlFor="estado">Estado:</label><br />
                    <select
                        name="idEstado"
                        value={novoComprador.idEstado}
                        onChange={handleChange}
                        style={{ marginRight: '10px' }}
                        disabled={carregando}
                    >
                        <option value="">Selecione um estado</option>
                        {estados.map((uf) => (
                            <option key={uf.id} value={uf.id}>
                                {uf.nome}
                            </option>
                        ))}
                    </select>
                </div>
                <div>
                    <label htmlFor="cidade">Cidade:</label><br />
                    <select
                        name="idCidade"
                        value={novoComprador.idCidade}
                        onChange={handleChange}
                        disabled={carregando}
                    >
                        <option value="">Selecione uma cidade</option>
                        {cidades.map((cidade) => (
                            <option key={cidade.id} value={cidade.id}>
                                {cidade.nome}
                            </option>
                        ))}
                    </select>
                </div>
            </div>

            <button onClick={modoEdicao ? salvarEdicao : adicionarComprador} disabled={carregando}>
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

export default Compradores;
