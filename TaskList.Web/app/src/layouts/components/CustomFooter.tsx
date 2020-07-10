import * as React from 'react';
import { Layout, Typography, } from 'antd';
import { ReactComponent as LogoSeal } from 'assets/logo-seal.svg';

const footerLogo = require('assets/footer.png');

const { Footer } = Layout;

class CustomFooter extends React.Component<{}, {}> {
    render() {
        return (
            <Footer className="footer">

                <div style={{
                    position: 'absolute',
                    top: '20px',
                    left: '20px',
                }}>
                    <LogoSeal className="footer-seal" />
                    <Typography.Title style={{
                        marginLeft: '120px'
                    }}>
                        Continuity of Service
                    </Typography.Title>

                    <Typography.Title level={4} style={{
                        fontWeight: 'bold',
                        color: '#ffffff',
                        marginLeft: '190px',
                        fontSize: '18px'
                    }}>
                        TOS Office Wide Goals
                    </Typography.Title>

                    <ul style={{
                        color: '#ffffff',
                        listStyle: 'none',
                        fontFamily: 'Didact Gothic, sans-serif',
                        fontSize: '14px',
                        marginLeft: '80px',
                    }}>
                        <li>&#10146;&nbsp;&nbsp;Promote and maintain the public trust and confidence.</li>
                        <li>&#10146;&nbsp;&nbsp;Provide quality services and information.</li>
                        <li>&#10146;&nbsp;&nbsp;Promote positive relationships and prudent practices.</li>
                        <li>&#10146;&nbsp;&nbsp;Promote a supportive work environment.</li>
                    </ul>
                </div>

                <img src={footerLogo} alt="Continuity of Service Logo" className="footer-logo" />
            </Footer>
        );
    }
}

export default CustomFooter;
