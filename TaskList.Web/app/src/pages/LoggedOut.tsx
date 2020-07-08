import * as React from 'react';

class LoggedOut extends React.Component<{}, {}> {
    render() {
        return (
            <div style={{ padding: '24px' }}>
                <h2>Successfully logged out</h2>
                {/* <Button type="primary" onClick={this.login}>Login with Azure AD</Button> */}
            </div>);
    }
}

export default LoggedOut;
