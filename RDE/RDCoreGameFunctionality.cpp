// Move this file to GDE/RDCoreGameFunctionality/RDCoreGameFunctionality.cpp

#include <class_db.hpp>
#include <gdextension_interface.h>
#include <node.hpp>
#include <defs.hpp>
#include <extension.hpp>

using namespace godot;

class RDCoreGameFunctionality : public Node {
    GDCLASS(RDCoreGameFunctionality, Node);

public:
    RDCoreGameFunctionality() {}
    ~RDCoreGameFunctionality() {}

    void _ready() {
        // Example functionality
        Godot::print("RDCoreGameFunctionality is ready!");
    }
};

void gdextension_rdcoregamefunctionality_init(GDExtensionInterfaceGetProcAddress get_proc_address, GDExtensionClassLibraryPtr library, GDExtensionInitialization *initialization) {
    godot::GDExtensionBinding::InitObject init_obj(get_proc_address, library, initialization);
    init_obj.register_class<RDCoreGameFunctionality>();
}
