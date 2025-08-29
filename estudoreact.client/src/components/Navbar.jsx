import React from 'react';
import { Link } from 'react-router-dom';
import '../App.css';

const Navbar = () => {
    return (
        <div style={{ padding: '20px', fontFamily: 'Arial' }}>
            <h1>🏠 Menu Principal</h1>
            <nav style={{ marginBottom: '20px' }}>
                <ul>
                    <li><Link to="/compradores">Compradores</Link></li>
                    <li><Link to="/produtos">Produtos</Link></li>
                    <li><Link to="/pedidos">Pedidos</Link></li>
                </ul>
            </nav>
        </div>
    );
};

export default Navbar;
