const sendMessage = (type, body) => {
    window.external.sendMessage(JSON.stringify({Type:type, Body: body}))
}

const dispatcher = new Map()

const onMsg = (type, callback) => {
    dispatcher[type] = callback
}

window.external.receiveMessage(message => {
    const msg = JSON.parse(message)
    dispatcher[msg.Type](msg.Body)
})

window.addEventListener('load', (e)=> {
    let editor = null;

    /*
    {
  "time": 1644789920570,
  "blocks": [
    {
      "id": "Y0_lwKAW6i",
      "type": "table",
      "data": {
        "withHeadings": false,
        "content": [
          [
            "test",
            "hoge"
          ]
        ]
      }
    }
  ],
  "version": "2.23.2"
}
    */

    document.getElementById("save-button").addEventListener('click', (e)=>{
        editor.save()
            .then((data)=> sendMessage("notifyDone", JSON.stringify(data.blocks[0].data)))
            // .then((data)=> sendMessage("notifyDeb", JSON.stringify(data.blocks[0].data, null, 2)))
            .catch((err)=> sendMessage("notifyDeb", err))
    })
    document.getElementById('cancel-button').addEventListener('click', ()=>sendMessage('notifyCancel', ""))

    onMsg("loadTable", (tablejson) => {

        editor = new EditorJS({holder:'editor-root',
            tools: {
                table: {
                    class: Table,
                    inlineToolbar: false,
                    config: {
                        rows: 1,
                        cols: 2
                    }
                }
            },
            autofocus: true,
            data: {
                blocks: [
                    {
                        type: "table",
                        data: JSON.parse(tablejson)
                        /*{
                            "content" : [ [ "", ""]]
                        }*/
                }

                ]
            }});
    })

    sendMessage("notifyLoaded", "")
})

