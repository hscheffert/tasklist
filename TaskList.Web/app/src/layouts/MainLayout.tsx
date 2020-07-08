import * as React from 'react';
import SideNav from '../sideNav/SideNav';
import { Layout } from 'antd';
import { RouteComponentProps } from 'react-router-dom';
import SimpleHeader from './components/SimpleHeader';

const { Header, Content } = Layout;

interface FullscreenAdminLayoutState {
    collapsed: boolean;
}

class FullscreenLayout extends React.Component<RouteComponentProps<{}>, FullscreenAdminLayoutState>  {
    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.state = {
            collapsed: false
        };
    }

    toggle = () => {
        this.setState({
            collapsed: !this.state.collapsed,
        });
    }

    render() {
        return (
            <Layout className="layout">
                <SimpleHeader />

                <Layout>
                    <SideNav />
                    <Layout style={{ padding: '24px' }}>
                        <Content
                            className="content-background"
                            style={{
                                margin: 0,
                            }}>
                            {this.props.children}
                        </Content>
                    </Layout>
                </Layout>
            </Layout>
        );
    }
}

export default FullscreenLayout;
