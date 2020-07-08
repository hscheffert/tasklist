import * as React from 'react';
import { Input, Button } from 'antd';
import Icon from '@ant-design/icons';

/**
 * **NOTE: Please use 'getColumnSearchProps()' unless you need dot notation**
 * This function simply removes the 'nameof' constraint and allows for dot notation.
 * Use this function just like you would the getColumnSearchProps() function
 * It allows you to provide a dot notated **dataIndex**
 *
 *    getColumnSearchPropsHard("content.name", "Name") OR getColumnSearchPropsHard("course.lesson.id")
 *
 * **Usage:** Place in the ColumnProds of an AntD Table like this -> `...getColumnSearchPropsAdvanced("role.name", "Role")`
 *
 * @param {string} dataIndex
 * @returns
 */
export function getColumnSearchPropsAdvanced(dataIndex: string, friendlyName?: string) {
    // 'Borrowed' and expanded from https://ant.design/components/table/#components-table-demo-custom-filter-panel
    let searchInput: any;
    return {
        filterDropdown: ({ setSelectedKeys, selectedKeys, confirm, clearFilters }: any) => {
            return <div style={{ padding: 12 }}>
                <Input
                    ref={node => { searchInput = node; }} // This selects the text after opening the filter
                    placeholder={`Search ${friendlyName || dataIndex.split(".").slice(-1)}`}
                    value={selectedKeys[0]}
                    onChange={e => setSelectedKeys(e.target.value ? [e.target.value] : [])}
                    onPressEnter={() => confirm()}
                    style={{ width: 196, marginBottom: 8, display: 'block' }}
                />
                <Button
                    type="primary"
                    onClick={() => confirm()}
                    icon="search"
                    style={{ width: 92, marginRight: 12 }}
                >
                    Search
                </Button>
                <Button
                    onClick={() => clearFilters()}
                    style={{ width: 92 }}
                >
                    Reset
            </Button>
            </div>;
        },
        filterIcon: (filtered: any) => <Icon type="search" style={{ color: filtered ? '#1890ff' : undefined }} />,
        onFilter: (value: string, record: any) => {
            // Since dataIndex can be '.' notated, we need to dive into the record
            dataIndex.split(".").forEach(x => {
                if (record == null) {
                    // We cannot go further!
                    console.error("Failed to recurse record", dataIndex, record);
                    return true;
                }
                record = record[x];
            });
            let recordString: string = record != null ? record.toString() : "";
            return recordString.toLowerCase().includes(value.toLowerCase());
        },
        onFilterDropdownVisibleChange: (visible: any) => {
            if (visible) {
                setTimeout(() => searchInput.select());
            }
        },
    };
}

/**
 * A simple helper function that returns the required search functions for a given table column.
 * This will allow full text searching a particular column
 *
 * Uses the 'nameof' principal and will pull in your model's keys for intellisense.
 *
 * **Usage:** Place in the ColumnProds of an AntD Table like this -> `...getColumnSearchProps<TableModel>("email", "Email Address")`
 *
 * @param {keyof T} dataIndex This will match with your column dataIndex
 * @returns
 */
export function getColumnSearchProps<T>(dataIndex: keyof T, friendlyName?: string) {
    return getColumnSearchPropsAdvanced(dataIndex.toString(), friendlyName);
}
